namespace sc2
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.starCraftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDrawGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDrawPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDrawValue = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDrawTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlView = new System.Windows.Forms.Panel();
            this.picScreen = new System.Windows.Forms.PictureBox();
            this.tvGameState = new System.Windows.Forms.TreeView();
            this.picScreenToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnlView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.starCraftToolStripMenuItem,
            this.mnuOption,
            this.viewOptionToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(867, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // starCraftToolStripMenuItem
            // 
            this.starCraftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.sendCommandToolStripMenuItem,
            this.saveStateToolStripMenuItem,
            this.loadGameStateToolStripMenuItem});
            this.starCraftToolStripMenuItem.Name = "starCraftToolStripMenuItem";
            this.starCraftToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.starCraftToolStripMenuItem.Text = "StarCraft";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // sendCommandToolStripMenuItem
            // 
            this.sendCommandToolStripMenuItem.Name = "sendCommandToolStripMenuItem";
            this.sendCommandToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.sendCommandToolStripMenuItem.Text = "SendCommand";
            this.sendCommandToolStripMenuItem.Click += new System.EventHandler(this.sendCommandToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.saveStateToolStripMenuItem.Text = "Save State";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // loadGameStateToolStripMenuItem
            // 
            this.loadGameStateToolStripMenuItem.Name = "loadGameStateToolStripMenuItem";
            this.loadGameStateToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.loadGameStateToolStripMenuItem.Text = "Load GameState";
            this.loadGameStateToolStripMenuItem.Click += new System.EventHandler(this.loadGameStateToolStripMenuItem_Click);
            // 
            // mnuOption
            // 
            this.mnuOption.Name = "mnuOption";
            this.mnuOption.Size = new System.Drawing.Size(67, 24);
            this.mnuOption.Text = "Option";
            // 
            // viewOptionToolStripMenuItem
            // 
            this.viewOptionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkDrawGrid,
            this.chkDrawPosition,
            this.chkDrawValue,
            this.chkDrawTarget});
            this.viewOptionToolStripMenuItem.Name = "viewOptionToolStripMenuItem";
            this.viewOptionToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.viewOptionToolStripMenuItem.Text = "ViewOption";
            // 
            // chkDrawGrid
            // 
            this.chkDrawGrid.CheckOnClick = true;
            this.chkDrawGrid.Name = "chkDrawGrid";
            this.chkDrawGrid.Size = new System.Drawing.Size(176, 26);
            this.chkDrawGrid.Text = "Draw Grid";
            this.chkDrawGrid.Click += new System.EventHandler(this.drawGridToolStripMenuItem_Click);
            // 
            // chkDrawPosition
            // 
            this.chkDrawPosition.CheckOnClick = true;
            this.chkDrawPosition.Name = "chkDrawPosition";
            this.chkDrawPosition.Size = new System.Drawing.Size(176, 26);
            this.chkDrawPosition.Text = "Draw Position";
            this.chkDrawPosition.Click += new System.EventHandler(this.drawGridToolStripMenuItem_Click);
            // 
            // chkDrawValue
            // 
            this.chkDrawValue.CheckOnClick = true;
            this.chkDrawValue.Name = "chkDrawValue";
            this.chkDrawValue.Size = new System.Drawing.Size(176, 26);
            this.chkDrawValue.Text = "Draw Value";
            this.chkDrawValue.Click += new System.EventHandler(this.drawGridToolStripMenuItem_Click);
            // 
            // chkDrawTarget
            // 
            this.chkDrawTarget.Checked = true;
            this.chkDrawTarget.CheckOnClick = true;
            this.chkDrawTarget.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawTarget.Name = "chkDrawTarget";
            this.chkDrawTarget.Size = new System.Drawing.Size(176, 26);
            this.chkDrawTarget.Text = "Draw Target";
            this.chkDrawTarget.Click += new System.EventHandler(this.drawGridToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test1ToolStripMenuItem,
            this.test2ToolStripMenuItem,
            this.test3ToolStripMenuItem,
            this.test4ToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // test1ToolStripMenuItem
            // 
            this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            this.test1ToolStripMenuItem.Size = new System.Drawing.Size(119, 26);
            this.test1ToolStripMenuItem.Text = "Test1";
            this.test1ToolStripMenuItem.Click += new System.EventHandler(this.test1ToolStripMenuItem_Click);
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size(119, 26);
            this.test2ToolStripMenuItem.Text = "Test2";
            this.test2ToolStripMenuItem.Click += new System.EventHandler(this.test2ToolStripMenuItem_Click);
            // 
            // test3ToolStripMenuItem
            // 
            this.test3ToolStripMenuItem.Name = "test3ToolStripMenuItem";
            this.test3ToolStripMenuItem.Size = new System.Drawing.Size(119, 26);
            this.test3ToolStripMenuItem.Text = "Test3";
            this.test3ToolStripMenuItem.Click += new System.EventHandler(this.test3ToolStripMenuItem_Click);
            // 
            // test4ToolStripMenuItem
            // 
            this.test4ToolStripMenuItem.Name = "test4ToolStripMenuItem";
            this.test4ToolStripMenuItem.Size = new System.Drawing.Size(119, 26);
            this.test4ToolStripMenuItem.Text = "Test4";
            this.test4ToolStripMenuItem.Click += new System.EventHandler(this.test4ToolStripMenuItem_Click);
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(12, 31);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(843, 22);
            this.txtInput.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Location = new System.Drawing.Point(12, 59);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(843, 440);
            this.tabMain.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.pnlView);
            this.tabPage1.Controls.Add(this.tvGameState);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(835, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Step File";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(236, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 35);
            this.button2.TabIndex = 5;
            this.button2.TabStop = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(200, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 35);
            this.button1.TabIndex = 4;
            this.button1.TabStop = false;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlView
            // 
            this.pnlView.AutoScroll = true;
            this.pnlView.Controls.Add(this.picScreen);
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlView.Location = new System.Drawing.Point(194, 3);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(638, 405);
            this.pnlView.TabIndex = 2;
            this.pnlView.TabStop = true;
            // 
            // picScreen
            // 
            this.picScreen.Location = new System.Drawing.Point(6, 3);
            this.picScreen.Name = "picScreen";
            this.picScreen.Size = new System.Drawing.Size(200, 200);
            this.picScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picScreen.TabIndex = 2;
            this.picScreen.TabStop = false;
            this.picScreen.MouseHover += new System.EventHandler(this.picScreen_MouseHover);
            this.picScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picScreen_MouseMove);
            // 
            // tvGameState
            // 
            this.tvGameState.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvGameState.ImageIndex = 0;
            this.tvGameState.ImageList = this.imageList1;
            this.tvGameState.Location = new System.Drawing.Point(3, 3);
            this.tvGameState.Name = "tvGameState";
            this.tvGameState.SelectedImageIndex = 0;
            this.tvGameState.Size = new System.Drawing.Size(191, 405);
            this.tvGameState.TabIndex = 3;
            this.tvGameState.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvGameState_AfterCollapse);
            this.tvGameState.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvGameState_AfterExpand);
            this.tvGameState.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGameState_AfterSelect);
            this.tvGameState.Click += new System.EventHandler(this.tvGameState_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder_16x.png");
            this.imageList1.Images.SetKeyName(1, "FolderOpen_16x.png");
            this.imageList1.Images.SetKeyName(2, "Script_16x.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 511);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem starCraftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendCommandToolStripMenuItem;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOption;
        private System.Windows.Forms.ToolStripMenuItem test3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test4ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem loadGameStateToolStripMenuItem;
        private System.Windows.Forms.TreeView tvGameState;
        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.ToolStripMenuItem viewOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chkDrawGrid;
        private System.Windows.Forms.ToolStripMenuItem chkDrawPosition;
        private System.Windows.Forms.ToolStripMenuItem chkDrawValue;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox picScreen;
        private System.Windows.Forms.ToolStripMenuItem chkDrawTarget;
        private System.Windows.Forms.ToolTip picScreenToolTip;
        private System.Windows.Forms.ImageList imageList1;
    }
}

