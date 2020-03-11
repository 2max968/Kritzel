namespace Kritzel.GUIElements
{
    partial class DocumentOverview
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
            this.list = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // list
            // 
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.HideSelection = false;
            this.list.Location = new System.Drawing.Point(0, 0);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(370, 483);
            this.list.TabIndex = 0;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.List;
            // 
            // DocumentOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.list);
            this.Name = "DocumentOverview";
            this.Size = new System.Drawing.Size(370, 483);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView list;
    }
}
