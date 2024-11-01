
namespace EvaluationUtility
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btnEvaluate = new System.Windows.Forms.Button();
            this._fbd = new System.Windows.Forms.FolderBrowserDialog();
            this._rtb = new System.Windows.Forms.RichTextBox();
            this._btnConfig = new System.Windows.Forms.Button();
            this._cbxPalette = new System.Windows.Forms.ComboBox();
            this._lblPalette = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _btnEvaluate
            // 
            this._btnEvaluate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnEvaluate.Location = new System.Drawing.Point(538, 343);
            this._btnEvaluate.Name = "_btnEvaluate";
            this._btnEvaluate.Size = new System.Drawing.Size(75, 30);
            this._btnEvaluate.TabIndex = 0;
            this._btnEvaluate.Text = "Evaluate";
            this._btnEvaluate.UseVisualStyleBackColor = true;
            this._btnEvaluate.Click += new System.EventHandler(this._btnEvaluate_Click);
            // 
            // _rtb
            // 
            this._rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rtb.Location = new System.Drawing.Point(12, 12);
            this._rtb.Name = "_rtb";
            this._rtb.Size = new System.Drawing.Size(601, 325);
            this._rtb.TabIndex = 1;
            this._rtb.Text = "";
            // 
            // _btnConfig
            // 
            this._btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnConfig.Location = new System.Drawing.Point(457, 343);
            this._btnConfig.Name = "_btnConfig";
            this._btnConfig.Size = new System.Drawing.Size(75, 30);
            this._btnConfig.TabIndex = 2;
            this._btnConfig.Text = "Config";
            this._btnConfig.UseVisualStyleBackColor = true;
            this._btnConfig.Click += new System.EventHandler(this._btnConfig_Click);
            // 
            // _cbxPalette
            // 
            this._cbxPalette.FormattingEnabled = true;
            this._cbxPalette.Location = new System.Drawing.Point(61, 349);
            this._cbxPalette.Name = "_cbxPalette";
            this._cbxPalette.Size = new System.Drawing.Size(121, 21);
            this._cbxPalette.TabIndex = 3;
            this._cbxPalette.SelectedIndexChanged += new System.EventHandler(this._cbxPalette_SelectedIndexChanged);
            // 
            // _lblPalette
            // 
            this._lblPalette.AutoSize = true;
            this._lblPalette.Location = new System.Drawing.Point(12, 352);
            this._lblPalette.Name = "_lblPalette";
            this._lblPalette.Size = new System.Drawing.Size(43, 13);
            this._lblPalette.TabIndex = 4;
            this._lblPalette.Text = "Palette:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 385);
            this.Controls.Add(this._lblPalette);
            this.Controls.Add(this._cbxPalette);
            this.Controls.Add(this._btnConfig);
            this.Controls.Add(this._rtb);
            this.Controls.Add(this._btnEvaluate);
            this.Name = "MainForm";
            this.Text = "WG ADR EVALUATION UTILITY";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnEvaluate;
        private System.Windows.Forms.FolderBrowserDialog _fbd;
        private System.Windows.Forms.RichTextBox _rtb;
        private System.Windows.Forms.Button _btnConfig;
        private System.Windows.Forms.ComboBox _cbxPalette;
        private System.Windows.Forms.Label _lblPalette;
    }
}

