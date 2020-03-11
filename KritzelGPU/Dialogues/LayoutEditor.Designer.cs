namespace Kritzel.Dialogues
{
    partial class LayoutEditor
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.backgroundSelectPanel1 = new Kritzel.GUIElements.BackgroundSelectPanel();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnC1 = new System.Windows.Forms.Button();
            this.btnC2 = new System.Windows.Forms.Button();
            this.cbShowDate = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.backgroundSelectPanel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 82);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(468, 273);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // backgroundSelectPanel1
            // 
            this.backgroundSelectPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.backgroundSelectPanel1.Location = new System.Drawing.Point(3, 3);
            this.backgroundSelectPanel1.Name = "backgroundSelectPanel1";
            this.backgroundSelectPanel1.Size = new System.Drawing.Size(424, 216);
            this.backgroundSelectPanel1.TabIndex = 0;
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.Location = new System.Drawing.Point(15, 12);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(78, 64);
            this.btnMinus.TabIndex = 1;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.Location = new System.Drawing.Point(402, 12);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(78, 64);
            this.btnPlus.TabIndex = 2;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(99, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 64);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnC1
            // 
            this.btnC1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnC1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnC1.Location = new System.Drawing.Point(15, 408);
            this.btnC1.Name = "btnC1";
            this.btnC1.Size = new System.Drawing.Size(209, 50);
            this.btnC1.TabIndex = 4;
            this.btnC1.Text = "Main Color";
            this.btnC1.UseVisualStyleBackColor = true;
            this.btnC1.Click += new System.EventHandler(this.btnC1_Click);
            // 
            // btnC2
            // 
            this.btnC2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnC2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnC2.Location = new System.Drawing.Point(271, 409);
            this.btnC2.Name = "btnC2";
            this.btnC2.Size = new System.Drawing.Size(209, 50);
            this.btnC2.TabIndex = 5;
            this.btnC2.Text = "Secondary Color";
            this.btnC2.UseVisualStyleBackColor = true;
            this.btnC2.Click += new System.EventHandler(this.btnC2_Click);
            // 
            // cbShowDate
            // 
            this.cbShowDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowDate.AutoSize = true;
            this.cbShowDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowDate.Location = new System.Drawing.Point(12, 361);
            this.cbShowDate.Name = "cbShowDate";
            this.cbShowDate.Size = new System.Drawing.Size(333, 41);
            this.cbShowDate.TabIndex = 1;
            this.cbShowDate.Text = "Show Creation Time";
            this.cbShowDate.UseVisualStyleBackColor = true;
            this.cbShowDate.CheckedChanged += new System.EventHandler(this.cbShowDate_CheckedChanged);
            // 
            // LayoutEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 470);
            this.Controls.Add(this.btnC2);
            this.Controls.Add(this.cbShowDate);
            this.Controls.Add(this.btnC1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayoutEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Layout";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private GUIElements.BackgroundSelectPanel backgroundSelectPanel1;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnC1;
        private System.Windows.Forms.Button btnC2;
        private System.Windows.Forms.CheckBox cbShowDate;
    }
}