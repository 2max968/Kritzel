namespace Kritzel.WebUI
{
    partial class WebDialog
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.web = new System.Windows.Forms.WebBrowser();
            this.ttSize = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // web
            // 
            this.web.AllowWebBrowserDrop = false;
            this.web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.web.IsWebBrowserContextMenuEnabled = false;
            this.web.Location = new System.Drawing.Point(0, 0);
            this.web.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.web.MinimumSize = new System.Drawing.Size(30, 31);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(662, 562);
            this.web.TabIndex = 0;
            this.web.Url = new System.Uri("", System.UriKind.Relative);
            this.web.WebBrowserShortcutsEnabled = false;
            // 
            // WebDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 562);
            this.Controls.Add(this.web);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WebDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.WebDialog_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.ToolTip ttSize;
    }
}

