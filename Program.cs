using IrtSecurityObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvaluationUtility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!CheckSecurity())
            {
                MessageBox.Show("You don't have permission to use this application.");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool CheckSecurity()
        {
            return true;
#if __DEBUG__
            return true;
#endif
            try
            {
                var secure = new IRTSecurity();
                var configStr = "";
                secure.ReadCSISecurity(ref configStr);
                if(configStr.Length < 50)
                {
                    return false;
                }
                if (configStr.Substring(49, 1) == "t")
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
