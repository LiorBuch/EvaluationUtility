using OfficeOpenXml;
using PatternRecognizer;
using PatternRecognizer.Evaluator;
using PatternRecognizer.SGSReader;
using PatternRecognizer.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvaluationUtility
{
    public partial class MainForm : Form
    {
        private bool _isRotoric;
        private static string _palettesPath = "palettes";
        public MainForm()
        {
            InitializeComponent();
            MultiFileEvaluator.Log = Log;
            _cbxPalette.Items.AddRange(GetPalettes());
            _cbxPalette.SelectedIndex = 0;
        }
        private string[] GetPalettes()
        {
            return Directory.GetFiles(_palettesPath).Select(str => new FileInfo(str).Name).ToArray();
        }
        private void _btnEvaluate_Click(object sender, EventArgs e)
        {
            if(_fbd.ShowDialog() == DialogResult.OK)
            {
                var defects = new MultiFileEvaluator().Evaluate(_fbd.SelectedPath);
                if (defects.Any())
                {
                    var ew = new SMExcelWrapper.ExcelWrapper();
                    var names = Enumerable.Range(0, defects.Length).Select(i => $"File {i + 1}").ToArray();
                    ew.Init(ref names);
                    for (var i = 0; i < names.Length; i++)
                    {
                        AddList(ew, i, defects[i].Item1, defects[i].Item2);
                    }
                    var fileName = Path.Combine(_fbd.SelectedPath, "report.xlsx");
                    ew.SaveFile(fileName);
                    doFormat(_fbd.SelectedPath);

                    if (MessageBox.Show($"Report is saved to {fileName}. Do you need to open it?", "Report is saved", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new FileInfo(fileName).FullName);
                    }
                }
            }
        }

        private void AddList(SMExcelWrapper.ExcelWrapper ew, int i, string fileName, DefectsSet ds)
        {
            var defects = ds.Defects;
            if (!defects.Any())
            {
                return;
            }
            _isRotoric = defects[0].IsRotoric;
            ew.AddDataCell(i, 1, 1, fileName);
            ew.MergeCells(i, 1, 1, Headers.Length);
            for(var k = 0; k < Headers.Length; k++)
            {
                ew.AddDataCell(i, 2, k + 1, Headers[k]);
            }
            ew.SetFormat(i, 2, 1, Headers.Length + 1, true, false, Color.Black);
            var lines = new List<string>();
            lines.Add(string.Join(",", Headers));
            for (var j = 0; j < defects.Count; j++)
            {
                var row = GetData(defects[j]);
                for (var k = 0; k < row.Length; k++)
                {
                    ew.AddDataCell(i, j + 3, k + 1, row[k]);
                }
                lines.Add(string.Join(",", row));
            }
            var csvFileName = Sgs2(fileName, ".csv");
            Log($"{csvFileName} is written.");
            File.WriteAllLines(csvFileName, lines);
            ew.AutoFitColumns(i);
            var csi = CScanImage.CreateCScanImage(ds, Context.Instance.Config.UnitName, _isRotoric);
            var pngFileName = Sgs2(fileName, ".png");
            Log($"{pngFileName} is written.");
            SaveHiddencontrol(csi, pngFileName);
        }
        void SaveHiddencontrol(Control ctl, string fileName)
        {
            ctl.Left = 22222;  // way outside
            ctl.Parent = this;
            var fmt = System.Drawing.Imaging.ImageFormat.Jpeg;
            if (fileName.ToLower().EndsWith(".png")) fmt = System.Drawing.Imaging.ImageFormat.Png;
            using (var bmp = new Bitmap(ctl.ClientSize.Width, ctl.ClientSize.Height))
            {
                ctl.DrawToBitmap(bmp, ctl.ClientRectangle);
                bmp.Save(fileName, fmt);
            }
        }
        private string Sgs2(string fileName, string extension)
        {
            fileName = fileName.Substring(0, fileName.Length - 4);
            fileName += "-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            return fileName + extension;
        }
        private string[] Headers => new[]
        {
            "No",
            "Type",
            "Average Amp",
            $"Min Depth({Context.Instance.Config.UnitName})",
            $"Max Depth({Context.Instance.Config.UnitName})",
            $"Depth for Max Amp({Context.Instance.Config.UnitName})",
            $"Start Line({Context.Instance.Config.UnitName})",
            $"End Line({Context.Instance.Config.UnitName})",
            _isRotoric ? "Start Degree" : $"Start Column({Context.Instance.Config.UnitName})",
            _isRotoric ? "End Degree" : $"End Column({Context.Instance.Config.UnitName})",
            $"Width({Context.Instance.Config.UnitName})",
            $"Height({Context.Instance.Config.UnitName})"
        };
        private object[] GetData(Defect d)
        {
            var format = Context.Instance.Config.EvaluationUnit == 0 ? "0.000" : "0.00";
            Func<double, string> f = dd => (dd / Context.Instance.Config.K).ToString(format);
            return new object[]
            {
                d.No,
                d.Type,
                d.Points.Average(p => p.Amp).ToString("0.00"),
                f(d.Points.Min(p => p.ToF)),
                f(d.Points.Max(p => p.ToF)),
                f(d.Points.OrderByDescending(p => p.Amp).First().ToF),
                f(d.MinLineY),
                f(d.MaxLineY),
                _isRotoric ? d.MinDegree.ToString("0.00") : f(d.MinColumnX),
                _isRotoric ? d.MaxDegree.ToString("0.00") : f(d.MaxColumnX),
                f(d.Width),
                f(d.Height)
            };
        }

        private void Log(string str)
        {
            _rtb.Text += str + Environment.NewLine;
        }

        private void _btnConfig_Click(object sender, EventArgs e)
        {
            new ConfigForm("EvalUtility").ShowDialog();
        }

        private void _cbxPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            CScanImage.PalettePath = new FileInfo(Path.Combine(Application.StartupPath, _palettesPath, _cbxPalette.SelectedItem.ToString())).FullName;
        }

        public void doFormat(string inPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var pack = new ExcelPackage(new FileInfo(inPath));
            var packOut = new ExcelPackage();
            var sheetOut = packOut.Workbook.Worksheets.Add("build");
            var sheet = pack.Workbook.Worksheets.First();
            List<int> getList = new List<int> { 1, 9, 7, 5, 3, 11 }; // the columns to take from the csv
            var j = 1; // the column in the new List
            List<string> colName = new List<string> { "Cluster", "T(Degree)", "X(mm)", "THK(mm)", "Ave.(%)", "Size(mm)" }; // initial data.
            var maxLen = 0;
            foreach (int i in getList)
            {
                sheetOut.Cells[1, j].Value = colName[j - 1];
                for (int z = 2; z < 300; z++) // z is for the rows indexing,starting from 2 because the first row is the names.
                {
                    if (sheet.Cells[z, i].Value is null)
                    {
                        maxLen = z; break; // maxLen defines the last row that can be reached to
                    }
                    if (i == 5)
                    {
                        sheetOut.Cells[z, j].Value = ((double)sheet.Cells[z, i].Value) - ((double)sheet.Cells[z, i - 1].Value);
                    }
                    if (i == 11) // the Size(mm)
                    {
                        sheetOut.Cells[z, j].Value = ((double)sheet.Cells[z, i].Value) * ((double)sheet.Cells[z, i + 1].Value);
                    }
                    sheetOut.Cells[z, j].Value = sheet.Cells[z, i].Value;
                }
                j = j + 1;
            }
            var img = sheetOut.Drawings.AddPicture("test", new FileInfo("C:\\Users\\LiorBuch\\Desktop\\sgs\\test\\moon.jpg")); //adds the cScan image
            img.SetPosition(1, 0, getList.Last(), 0);
            packOut.SaveAs(new FileInfo(Path.Combine(inPath, "Formated.xlsx"))); //save the new excel file
            
        }
    }
}
