namespace Kritzel.Main.GUIElements
{
    partial class FileMenu
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNew = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddPage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNew.Location = new System.Drawing.Point(0, 64);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(401, 75);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOpen.Location = new System.Drawing.Point(0, 139);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(401, 75);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.Location = new System.Drawing.Point(0, 214);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(401, 75);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveAs.Location = new System.Drawing.Point(0, 289);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(401, 75);
            this.btnSaveAs.TabIndex = 3;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClose.Image = global::Kritzel.Main.Properties.Resources.icoMenu;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(401, 64);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Menu";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddPage
            // 
            this.btnAddPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddPage.Location = new System.Drawing.Point(0, 364);
            this.btnAddPage.Name = "btnAddPage";
            this.btnAddPage.Size = new System.Drawing.Size(401, 75);
            this.btnAddPage.TabIndex = 5;
            this.btnAddPage.Text = "Add Page";
            this.btnAddPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddPage.UseVisualStyleBackColor = true;
            this.btnAddPage.Click += new System.EventHandler(this.btnAddPage_Click);
            // 
            // FileMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddPage);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnClose);
            this.Name = "FileMenu";
            this.Size = new System.Drawing.Size(401, 531);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddPage;
    }
}
