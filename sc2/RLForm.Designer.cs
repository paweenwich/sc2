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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newQLearningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSARSAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMenuETable = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(12, 60);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(415, 343);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(716, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newQLearningToolStripMenuItem,
            this.newSARSAToolStripMenuItem});
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.actionToolStripMenuItem.Text = "Action";
            // 
            // newQLearningToolStripMenuItem
            // 
            this.newQLearningToolStripMenuItem.Name = "newQLearningToolStripMenuItem";
            this.newQLearningToolStripMenuItem.Size = new System.Drawing.Size(186, 26);
            this.newQLearningToolStripMenuItem.Text = "New QLearning";
            this.newQLearningToolStripMenuItem.Click += new System.EventHandler(this.newQLearningToolStripMenuItem_Click);
            // 
            // newSARSAToolStripMenuItem
            // 
            this.newSARSAToolStripMenuItem.Name = "newSARSAToolStripMenuItem";
            this.newSARSAToolStripMenuItem.Size = new System.Drawing.Size(186, 26);
            this.newSARSAToolStripMenuItem.Text = "New SARSA";
            this.newSARSAToolStripMenuItem.Click += new System.EventHandler(this.newSARSAToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkMenuETable});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // chkMenuETable
            // 
            this.chkMenuETable.CheckOnClick = true;
            this.chkMenuETable.Name = "chkMenuETable";
            this.chkMenuETable.Size = new System.Drawing.Size(181, 26);
            this.chkMenuETable.Tag = "1";
            this.chkMenuETable.Text = "ETable";
            this.chkMenuETable.CheckedChanged += new System.EventHandler(this.qTableToolStripMenuItem_CheckedChanged);
            // 
            // RLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 606);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.picMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RLForm";
            this.Text = "RLForm";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newQLearningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSARSAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chkMenuETable;
    }
}