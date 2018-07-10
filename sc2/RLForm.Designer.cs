namespace sc2
{
    partial class RLForm
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
            this.picMain = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkEView = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(47, 57);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(415, 343);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkEView
            // 
            this.chkEView.AutoSize = true;
            this.chkEView.Location = new System.Drawing.Point(93, 14);
            this.chkEView.Name = "chkEView";
            this.chkEView.Size = new System.Drawing.Size(75, 21);
            this.chkEView.TabIndex = 2;
            this.chkEView.Text = "ETable";
            this.chkEView.UseVisualStyleBackColor = true;
            this.chkEView.CheckedChanged += new System.EventHandler(this.chkEView_CheckedChanged);
            // 
            // RLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 606);
            this.Controls.Add(this.chkEView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.picMain);
            this.Name = "RLForm";
            this.Text = "RLForm";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkEView;
    }
}