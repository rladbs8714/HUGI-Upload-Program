namespace Management
{
    partial class ShortKeySetMessegeBox
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
            this.TextBox = new System.Windows.Forms.TextBox();
            this.SaveAndClose = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(12, 12);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(480, 21);
            this.TextBox.TabIndex = 0;
            // 
            // SaveAndClose
            // 
            this.SaveAndClose.Location = new System.Drawing.Point(12, 39);
            this.SaveAndClose.Name = "SaveAndClose";
            this.SaveAndClose.Size = new System.Drawing.Size(235, 23);
            this.SaveAndClose.TabIndex = 1;
            this.SaveAndClose.Text = "저장하고 닫기";
            this.SaveAndClose.UseVisualStyleBackColor = true;
            this.SaveAndClose.Click += new System.EventHandler(this.SaveAndClose_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(257, 39);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(235, 23);
            this.Close.TabIndex = 2;
            this.Close.Text = "닫기";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // ShortKeySetMessegeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(505, 72);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.SaveAndClose);
            this.Controls.Add(this.TextBox);
            this.MaximizeBox = false;
            this.Name = "ShortKeySetMessegeBox";
            this.Text = "ShortKeySetMessegeBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShortKeySetMessegeBox_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox;
        private System.Windows.Forms.Button SaveAndClose;
        private System.Windows.Forms.Button Close;
    }
}