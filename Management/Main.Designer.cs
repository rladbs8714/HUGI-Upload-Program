namespace Management
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.StatusBox = new System.Windows.Forms.GroupBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.createButtonNumber = new System.Windows.Forms.NumericUpDown();
            this.CreateButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SetInputFormButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.StatusBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createButtonNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusBox
            // 
            this.StatusBox.Controls.Add(this.StatusLabel);
            this.StatusBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusBox.Location = new System.Drawing.Point(0, 581);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(770, 57);
            this.StatusBox.TabIndex = 0;
            this.StatusBox.TabStop = false;
            this.StatusBox.Text = "상태";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(375, 21);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(11, 12);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "-";
            // 
            // createButtonNumber
            // 
            this.createButtonNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.createButtonNumber.Location = new System.Drawing.Point(2, 1);
            this.createButtonNumber.Name = "createButtonNumber";
            this.createButtonNumber.Size = new System.Drawing.Size(120, 21);
            this.createButtonNumber.TabIndex = 3;
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(128, 0);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 4;
            this.CreateButton.Text = "버튼만들기";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButtonsButton);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(770, 557);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // SetInputFormButton
            // 
            this.SetInputFormButton.Location = new System.Drawing.Point(210, 0);
            this.SetInputFormButton.Name = "SetInputFormButton";
            this.SetInputFormButton.Size = new System.Drawing.Size(75, 23);
            this.SetInputFormButton.TabIndex = 5;
            this.SetInputFormButton.Text = "글 설정";
            this.SetInputFormButton.UseVisualStyleBackColor = true;
            this.SetInputFormButton.Click += new System.EventHandler(this.SetInputFormButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(770, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(770, 638);
            this.Controls.Add(this.SetInputFormButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.createButtonNumber);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.StatusBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Close);
            this.StatusBox.ResumeLayout(false);
            this.StatusBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createButtonNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox StatusBox;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.NumericUpDown createButtonNumber;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button SetInputFormButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

