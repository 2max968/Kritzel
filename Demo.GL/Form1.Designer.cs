namespace Demo.GL
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startAsyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.initGlewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 474);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startSyncToolStripMenuItem,
            this.startAsyncToolStripMenuItem,
            this.initGlewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(718, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // startSyncToolStripMenuItem
            // 
            this.startSyncToolStripMenuItem.Name = "startSyncToolStripMenuItem";
            this.startSyncToolStripMenuItem.Size = new System.Drawing.Size(101, 29);
            this.startSyncToolStripMenuItem.Text = "Start Sync";
            this.startSyncToolStripMenuItem.Click += new System.EventHandler(this.startSyncToolStripMenuItem_Click);
            // 
            // startAsyncToolStripMenuItem
            // 
            this.startAsyncToolStripMenuItem.Name = "startAsyncToolStripMenuItem";
            this.startAsyncToolStripMenuItem.Size = new System.Drawing.Size(112, 29);
            this.startAsyncToolStripMenuItem.Text = "Start Async";
            this.startAsyncToolStripMenuItem.Click += new System.EventHandler(this.startAsyncToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // initGlewToolStripMenuItem
            // 
            this.initGlewToolStripMenuItem.Enabled = false;
            this.initGlewToolStripMenuItem.Name = "initGlewToolStripMenuItem";
            this.initGlewToolStripMenuItem.Size = new System.Drawing.Size(92, 29);
            this.initGlewToolStripMenuItem.Text = "Init Glew";
            this.initGlewToolStripMenuItem.Click += new System.EventHandler(this.initGlewToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startSyncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startAsyncToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem initGlewToolStripMenuItem;
    }
}

