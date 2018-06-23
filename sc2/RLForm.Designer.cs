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
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(34, 35);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(415, 343);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            // 
            // RLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 606);
            this.Controls.Add(this.picMain);
            this.Name = "RLForm";
            this.Text = "RLForm";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
    }
}