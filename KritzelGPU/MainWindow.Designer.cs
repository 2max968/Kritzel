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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelTopMid = new System.Windows.Forms.Panel();
            this.colorPicker1 = new Kritzel.GUIElements.ColorPicker();
            this.panelTopRight = new System.Windows.Forms.Panel();
            this.btnFormType = new System.Windows.Forms.Button();
            this.btnLayout = new System.Windows.Forms.Button();
            this.btnFullscreen = new System.Windows.Forms.Button();
            this.panelTopLeft = new System.Windows.Forms.Panel();
            this.panelPage = new System.Windows.Forms.Panel();
            this.btnFile = new System.Windows.Forms.Button();
            this.menuFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFormType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.strokeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagOpenDoc = new System.Windows.Forms.OpenFileDialog();
            this.pageSelect = new Kritzel.PickerMenu();
            this.pmControl = new Kritzel.PickerMenu();
            this.pickerMenu1 = new Kritzel.PickerMenu();
            this.inkControl1 = new Kritzel.InkControl();
            this.panelTop.SuspendLayout();
            this.panelTopMid.SuspendLayout();
            this.panelTopRight.SuspendLayout();
            this.panelTopLeft.SuspendLayout();
            this.menuFiles.SuspendLayout();
            this.menuFormType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inkControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelTopMid);
            this.panelTop.Controls.Add(this.panelTopRight);
            this.panelTop.Controls.Add(this.panelTopLeft);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(876, 64);
            this.panelTop.TabIndex = 9;
            // 
            // panelTopMid
            // 
            this.panelTopMid.Controls.Add(this.colorPicker1);
            this.panelTopMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopMid.Location = new System.Drawing.Point(64, 0);
            this.panelTopMid.Name = "panelTopMid";
            this.panelTopMid.Size = new System.Drawing.Size(620, 64);
            this.panelTopMid.TabIndex = 2;
            // 
            // colorPicker1
            // 
            this.colorPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorPicker1.Location = new System.Drawing.Point(0, 0);
            this.colorPicker1.Name = "colorPicker1";
            this.colorPicker1.Size = new System.Drawing.Size(620, 64);
            this.colorPicker1.TabIndex = 0;
            // 
            // panelTopRight
            // 
            this.panelTopRight.AutoSize = true;
            this.panelTopRight.Controls.Add(this.btnFormType);
            this.panelTopRight.Controls.Add(this.btnLayout);
            this.panelTopRight.Controls.Add(this.btnFullscreen);
            this.panelTopRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelTopRight.Location = new System.Drawing.Point(684, 0);
            this.panelTopRight.Name = "panelTopRight";
            this.panelTopRight.Size = new System.Drawing.Size(192, 64);
            this.panelTopRight.TabIndex = 1;
            // 
            // btnFormType
            // 
            this.btnFormType.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFormType.Image = global::Kritzel.Properties.Resources.Pen;
            this.btnFormType.Location = new System.Drawing.Point(0, 0);
            this.btnFormType.Name = "btnFormType";
            this.btnFormType.Size = new System.Drawing.Size(64, 64);
            this.btnFormType.TabIndex = 2;
            this.btnFormType.UseVisualStyleBackColor = true;
            this.btnFormType.Click += new System.EventHandler(this.btnFormType_Click);
            // 
            // btnLayout
            // 
            this.btnLayout.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLayout.Image = global::Kritzel.Properties.Resources.page;
            this.btnLayout.Location = new System.Drawing.Point(64, 0);
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.Size = new System.Drawing.Size(64, 64);
            this.btnLayout.TabIndex = 1;
            this.btnLayout.UseVisualStyleBackColor = true;
            this.btnLayout.Click += new System.EventHandler(this.btnLayout_Click);
            // 
            // btnFullscreen
            // 
            this.btnFullscreen.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFullscreen.Image = global::Kritzel.Properties.Resources.ArrowsExpand;
            this.btnFullscreen.Location = new System.Drawing.Point(128, 0);
            this.btnFullscreen.Name = "btnFullscreen";
            this.btnFullscreen.Size = new System.Drawing.Size(64, 64);
            this.btnFullscreen.TabIndex = 0;
            this.btnFullscreen.UseVisualStyleBackColor = true;
            this.btnFullscreen.Click += new System.EventHandler(this.btnFullscreen_Click);
            // 
            // panelTopLeft
            // 
            this.panelTopLeft.AutoSize = true;
            this.panelTopLeft.Controls.Add(this.panelPage);
            this.panelTopLeft.Controls.Add(this.btnFile);
            this.panelTopLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTopLeft.Location = new System.Drawing.Point(0, 0);
            this.panelTopLeft.Name = "panelTopLeft";
            this.panelTopLeft.Size = new System.Drawing.Size(64, 64);
            this.panelTopLeft.TabIndex = 0;
            // 
            // panelPage
            // 
            this.panelPage.AutoSize = true;
            this.panelPage.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelPage.Location = new System.Drawing.Point(64, 0);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(0, 64);
            this.panelPage.TabIndex = 1;
            // 
            // btnFile
            // 
            this.btnFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnFile.Image = global::Kritzel.Properties.Resources.icoMenu;
            this.btnFile.Location = new System.Drawing.Point(0, 0);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(64, 64);
            this.btnFile.TabIndex = 0;
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // menuFiles
            // 
            this.menuFiles.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem});
            this.menuFiles.Name = "menuFiles";
            this.menuFiles.Size = new System.Drawing.Size(147, 124);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(146, 30);
            this.saveToolStripMenuItem1.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.saveAsToolStripMenuItem.Text = "&Save As";
            // 
            // menuFormType
            // 
            this.menuFormType.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuFormType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.strokeToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.rectangleToolStripMenuItem});
            this.menuFormType.Name = "menuFormType";
            this.menuFormType.Size = new System.Drawing.Size(169, 94);
            // 
            // strokeToolStripMenuItem
            // 
            this.strokeToolStripMenuItem.Image = global::Kritzel.Properties.Resources.Pen;
            this.strokeToolStripMenuItem.Name = "strokeToolStripMenuItem";
            this.strokeToolStripMenuItem.Size = new System.Drawing.Size(168, 30);
            this.strokeToolStripMenuItem.Text = "Stroke";
            this.strokeToolStripMenuItem.Click += new System.EventHandler(this.strokeToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Image = global::Kritzel.Properties.Resources.Line;
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(168, 30);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // rectangleToolStripMenuItem
            // 
            this.rectangleToolStripMenuItem.Image = global::Kritzel.Properties.Resources.Rect;
            this.rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
            this.rectangleToolStripMenuItem.Size = new System.Drawing.Size(168, 30);
            this.rectangleToolStripMenuItem.Text = "Rectangle";
            this.rectangleToolStripMenuItem.Click += new System.EventHandler(this.rectangleToolStripMenuItem_Click);
            // 
            // diagOpenDoc
            // 
            this.diagOpenDoc.Filter = "Supportet Files|*.zip;*.pdf;*.jpg;*.jpeg;*.png;*.bmp|Kritzel Documents|*.zip|PDF " +
    "Files|*.pdf|Images|*.jpg;*.jpeg;*.bmp;*.png";
            // 
            // pageSelect
            // 
            this.pageSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageSelect.Location = new System.Drawing.Point(12, 204);
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
            this.pmControl.Location = new System.Drawing.Point(800, 204);
            this.pmControl.Name = "pmControl";
            this.pmControl.Orientation = Kritzel.PickerOrientation.Vertical;
            this.pmControl.Selected = 0;
            this.pmControl.Size = new System.Drawing.Size(64, 256);
            this.pmControl.TabIndex = 7;
            this.pmControl.Type = Kritzel.PickerType.Check;
            // 
            // pickerMenu1
            // 
            this.pickerMenu1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pickerMenu1.Location = new System.Drawing.Point(12, 134);
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
            this.inkControl1.InkMode = Kritzel.InkMode.Pen;
            this.inkControl1.Location = new System.Drawing.Point(0, 64);
            this.inkControl1.LockDraw = false;
            this.inkControl1.LockMove = false;
            this.inkControl1.LockRotate = false;
            this.inkControl1.LockScale = false;
            this.inkControl1.Name = "inkControl1";
            this.inkControl1.Size = new System.Drawing.Size(876, 386);
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
            this.Controls.Add(this.pickerMenu1);
            this.Controls.Add(this.inkControl1);
            this.Controls.Add(this.panelTop);
            this.Name = "MainWindow";
            this.Text = "Kritzel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelTopMid.ResumeLayout(false);
            this.panelTopRight.ResumeLayout(false);
            this.panelTopLeft.ResumeLayout(false);
            this.panelTopLeft.PerformLayout();
            this.menuFiles.ResumeLayout(false);
            this.menuFormType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inkControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private InkControl inkControl1;
        private PickerMenu pickerMenu1;
        private PickerMenu pmControl;
        private PickerMenu pageSelect;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelTopRight;
        private System.Windows.Forms.Button btnFullscreen;
        private System.Windows.Forms.Panel panelTopLeft;
        private System.Windows.Forms.Button btnLayout;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.ContextMenuStrip menuFiles;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Button btnFormType;
        private System.Windows.Forms.ContextMenuStrip menuFormType;
        private System.Windows.Forms.ToolStripMenuItem strokeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectangleToolStripMenuItem;
        private System.Windows.Forms.Panel panelTopMid;
        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.OpenFileDialog diagOpenDoc;
        private GUIElements.ColorPicker colorPicker1;
    }
}