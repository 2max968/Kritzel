namespace Kritzel.Dialogues
{
    partial class PageAdder
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioBefore = new System.Windows.Forms.RadioButton();
            this.radioAfter = new System.Windows.Forms.RadioButton();
            this.radioEnd = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(412, 63);
            this.comboBox1.TabIndex = 0;
            // 
            // radioBefore
            // 
            this.radioBefore.AutoSize = true;
            this.radioBefore.Checked = true;
            this.radioBefore.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBefore.Location = new System.Drawing.Point(12, 81);
            this.radioBefore.Name = "radioBefore";
            this.radioBefore.Size = new System.Drawing.Size(327, 41);
            this.radioBefore.TabIndex = 1;
            this.radioBefore.TabStop = true;
            this.radioBefore.Text = "Before current Page";
            this.radioBefore.UseVisualStyleBackColor = true;
            // 
            // radioAfter
            // 
            this.radioAfter.AutoSize = true;
            this.radioAfter.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioAfter.Location = new System.Drawing.Point(12, 128);
            this.radioAfter.Name = "radioAfter";
            this.radioAfter.Size = new System.Drawing.Size(302, 41);
            this.radioAfter.TabIndex = 2;
            this.radioAfter.Text = "After current Page";
            this.radioAfter.UseVisualStyleBackColor = true;
            this.radioAfter.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioEnd
            // 
            this.radioEnd.AutoSize = true;
            this.radioEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioEnd.Location = new System.Drawing.Point(12, 175);
            this.radioEnd.Name = "radioEnd";
            this.radioEnd.Size = new System.Drawing.Size(290, 41);
            this.radioEnd.TabIndex = 3;
            this.radioEnd.Text = "End of Document";
            this.radioEnd.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(298, 235);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(126, 45);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // PageAdder
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 292);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.radioEnd);
            this.Controls.Add(this.radioAfter);
            this.Controls.Add(this.radioBefore);
            this.Controls.Add(this.comboBox1);
            this.Name = "PageAdder";
            this.Text = "PageAdder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioBefore;
        private System.Windows.Forms.RadioButton radioAfter;
        private System.Windows.Forms.RadioButton radioEnd;
        private System.Windows.Forms.Button btnAdd;
    }
}