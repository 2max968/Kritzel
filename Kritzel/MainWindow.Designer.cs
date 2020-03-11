namespace Kritzel
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleDOwnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageFormatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetRotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSelect = new Kritzel.PickerMenu();
            this.pmControl = new Kritzel.PickerMenu();
            this.pickerMenu3 = new Kritzel.PickerMenu();
            this.pickerMenu2 = new Kritzel.PickerMenu();
            this.pickerMenu1 = new Kritzel.PickerMenu();
            this.inkControl1 = new Kritzel.InkControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inkControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.backgroundToolStripMenuItem,
            this.layoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(876, 33);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exportToPDFToolStripMenuItem,
            this.importImageToolStripMenuItem,
            this.importPDFToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(206, 30);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(206, 30);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // exportToPDFToolStripMenuItem
            // 
            this.exportToPDFToolStripMenuItem.Name = "exportToPDFToolStripMenuItem";
            this.exportToPDFToolStripMenuItem.Size = new System.Drawing.Size(206, 30);
            this.exportToPDFToolStripMenuItem.Text = "Export to PDF";
            this.exportToPDFToolStripMenuItem.Click += new System.EventHandler(this.exportToPDFToolStripMenuItem_Click);
            // 
            // importImageToolStripMenuItem
            // 
            this.importImageToolStripMenuItem.Name = "importImageToolStripMenuItem";
            this.importImageToolStripMenuItem.Size = new System.Drawing.Size(206, 30);
            this.importImageToolStripMenuItem.Text = "Import Image";
            this.importImageToolStripMenuItem.Click += new System.EventHandler(this.importImageToolStripMenuItem_Click);
            // 
            // importPDFToolStripMenuItem
            // 
            this.importPDFToolStripMenuItem.Name = "importPDFToolStripMenuItem";
            this.importPDFToolStripMenuItem.Size = new System.Drawing.Size(206, 30);
            this.importPDFToolStripMenuItem.Text = "Import PDF";
            this.importPDFToolStripMenuItem.Click += new System.EventHandler(this.importPDFToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.scaleDOwnToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.fullscreenToolStripMenuItem.Text = "Fullscreen";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // scaleDOwnToolStripMenuItem
            // 
            this.scaleDOwnToolStripMenuItem.Name = "scaleDOwnToolStripMenuItem";
            this.scaleDOwnToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.scaleDOwnToolStripMenuItem.Text = "Scale Down";
            this.scaleDOwnToolStripMenuItem.Click += new System.EventHandler(this.scaleDOwnToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pageFormatsToolStripMenuItem,
            this.resetRotationToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(78, 29);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // pageFormatsToolStripMenuItem
            // 
            this.pageFormatsToolStripMenuItem.Name = "pageFormatsToolStripMenuItem";
            this.pageFormatsToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.pageFormatsToolStripMenuItem.Text = "Page Formats";
            this.pageFormatsToolStripMenuItem.Click += new System.EventHandler(this.pageFormatsToolStripMenuItem_Click);
            // 
            // resetRotationToolStripMenuItem
            // 
            this.resetRotationToolStripMenuItem.Name = "resetRotationToolStripMenuItem";
            this.resetRotationToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.resetRotationToolStripMenuItem.Text = "Reset Rotation";
            this.resetRotationToolStripMenuItem.Click += new System.EventHandler(this.resetRotationToolStripMenuItem_Click);
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(119, 29);
            this.backgroundToolStripMenuItem.Text = "Background";
            this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.backgroundToolStripMenuItem_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(77, 29);
            this.layoutToolStripMenuItem.Text = "Layout";
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutToolStripMenuItem_Click);
            // 
            // pageSelect
            // 
            this.pageSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageSelect.Location = new System.Drawing.Point(12, 173);
            this.pageSelect.Name = "pageSelect";
            this.pageSelect.Orientation = Kritzel.PickerOrientation.Vertical;
            this.pageSelect.Selected = 0;
            this.pageSelect.Size = new System.Drawing.Size(64, 256);
            this.pageSelect.TabIndex = 8;
            this.pageSelect.Type = Kritzel.PickerType.Click;
            // 
            // pmControl
            // 
            this.pmControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pmControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pmControl.Location = new System.Drawing.Point(800, 173);
            this.pmControl.Name = "pmControl";
            this.pmControl.Orientation = Kritzel.PickerOrientation.Vertical;
            this.pmControl.Selected = 0;
            this.pmControl.Size = new System.Drawing.Size(64, 256);
            this.pmControl.TabIndex = 7;
            this.pmControl.Type = Kritzel.PickerType.Check;
            // 
            // pickerMenu3
            // 
            this.pickerMenu3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pickerMenu3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pickerMenu3.Location = new System.Drawing.Point(12, 374);
            this.pickerMenu3.Name = "pickerMenu3";
            this.pickerMenu3.Orientation = Kritzel.PickerOrientation.Horizontal;
            this.pickerMenu3.Selected = 0;
            this.pickerMenu3.Size = new System.Drawing.Size(256, 64);
            this.pickerMenu3.TabIndex = 5;
            this.pickerMenu3.Type = Kritzel.PickerType.Select;
            // 
            // pickerMenu2
            // 
            this.pickerMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pickerMenu2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pickerMenu2.Location = new System.Drawing.Point(608, 103);
            this.pickerMenu2.Name = "pickerMenu2";
            this.pickerMenu2.Orientation = Kritzel.PickerOrientation.Horizontal;
            this.pickerMenu2.Selected = 0;
            this.pickerMenu2.Size = new System.Drawing.Size(256, 64);
            this.pickerMenu2.TabIndex = 4;
            this.pickerMenu2.Type = Kritzel.PickerType.Select;
            this.pickerMenu2.Load += new System.EventHandler(this.pickerMenu2_Load);
            // 
            // pickerMenu1
            // 
            this.pickerMenu1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pickerMenu1.Location = new System.Drawing.Point(12, 103);
            this.pickerMenu1.Name = "pickerMenu1";
            this.pickerMenu1.Orientation = Kritzel.PickerOrientation.Horizontal;
            this.pickerMenu1.Selected = 0;
            this.pickerMenu1.Size = new System.Drawing.Size(256, 64);
            this.pickerMenu1.TabIndex = 1;
            this.pickerMenu1.Type = Kritzel.PickerType.Select;
            // 
            // inkControl1
            // 
            this.inkControl1.BufferSize = 1F;
            this.inkControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inkControl1.Image = ((System.Drawing.Image)(resources.GetObject("inkControl1.Image")));
            this.inkControl1.InkMode = Kritzel.InkMode.Pen;
            this.inkControl1.Location = new System.Drawing.Point(0, 33);
            this.inkControl1.LockMove = false;
            this.inkControl1.LockRotate = false;
            this.inkControl1.LockScale = false;
            this.inkControl1.Name = "inkControl1";
            this.inkControl1.Size = new System.Drawing.Size(876, 417);
            this.inkControl1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.inkControl1.TabIndex = 0;
            this.inkControl1.TabStop = false;
            this.inkControl1.Thicknes = 5F;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 450);
            this.Controls.Add(this.pageSelect);
            this.Controls.Add(this.pmControl);
            this.Controls.Add(this.pickerMenu3);
            this.Controls.Add(this.pickerMenu2);
            this.Controls.Add(this.pickerMenu1);
            this.Controls.Add(this.inkControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "45 - 0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inkControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InkControl inkControl1;
        private PickerMenu pickerMenu1;
        private PickerMenu pickerMenu2;
        private PickerMenu pickerMenu3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageFormatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPDFToolStripMenuItem;
        private PickerMenu pmControl;
        private System.Windows.Forms.ToolStripMenuItem scaleDOwnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetRotationToolStripMenuItem;
        private PickerMenu pageSelect;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
    }
}