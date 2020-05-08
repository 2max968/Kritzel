namespace Kritzel.Main.GUIElements
{
    partial class ColorPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelSplitL = new System.Windows.Forms.Panel();
            this.panelPlitterR = new System.Windows.Forms.Panel();
            this.penCtx = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.penCtx.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExpand
            // 
            this.btnExpand.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExpand.Location = new System.Drawing.Point(424, 0);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(64, 64);
            this.btnExpand.TabIndex = 0;
            this.btnExpand.Text = "\\/";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAdd.Location = new System.Drawing.Point(4, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(64, 64);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelSplitL
            // 
            this.panelSplitL.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelSplitL.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSplitL.Location = new System.Drawing.Point(0, 0);
            this.panelSplitL.Name = "panelSplitL";
            this.panelSplitL.Size = new System.Drawing.Size(4, 64);
            this.panelSplitL.TabIndex = 2;
            // 
            // panelPlitterR
            // 
            this.panelPlitterR.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelPlitterR.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelPlitterR.Location = new System.Drawing.Point(488, 0);
            this.panelPlitterR.Name = "panelPlitterR";
            this.panelPlitterR.Size = new System.Drawing.Size(4, 64);
            this.panelPlitterR.TabIndex = 3;
            // 
            // penCtx
            // 
            this.penCtx.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.penCtx.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.penCtx.Name = "penCtx";
            this.penCtx.Size = new System.Drawing.Size(193, 64);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.changeToolStripMenuItem.Text = "Change Color";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.removeToolStripMenuItem.Text = "Remove Pen";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.panelSplitL);
            this.Controls.Add(this.panelPlitterR);
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(492, 64);
            this.penCtx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panelSplitL;
        private System.Windows.Forms.Panel panelPlitterR;
        private System.Windows.Forms.ContextMenuStrip penCtx;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}
