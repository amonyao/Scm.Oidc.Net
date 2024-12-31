namespace Com.Scm.Oidc.Win
{
    partial class Email
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            LtEmail = new Label();
            TbEmail = new TextBox();
            LtSms = new Label();
            TbSms = new TextBox();
            BtSms = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // LtEmail
            // 
            LtEmail.AutoSize = true;
            LtEmail.Location = new Point(3, 6);
            LtEmail.Name = "LtEmail";
            LtEmail.Size = new Size(56, 17);
            LtEmail.TabIndex = 0;
            LtEmail.Text = "电子邮件";
            // 
            // TbEmail
            // 
            TbEmail.Location = new Point(65, 3);
            TbEmail.Name = "TbEmail";
            TbEmail.Size = new Size(132, 23);
            TbEmail.TabIndex = 1;
            // 
            // LtSms
            // 
            LtSms.AutoSize = true;
            LtSms.Location = new Point(15, 35);
            LtSms.Name = "LtSms";
            LtSms.Size = new Size(44, 17);
            LtSms.TabIndex = 2;
            LtSms.Text = "验证码";
            // 
            // TbSms
            // 
            TbSms.Location = new Point(65, 32);
            TbSms.Name = "TbSms";
            TbSms.Size = new Size(71, 23);
            TbSms.TabIndex = 3;
            // 
            // BtSms
            // 
            BtSms.FlatStyle = FlatStyle.Popup;
            BtSms.Location = new Point(142, 32);
            BtSms.Name = "BtSms";
            BtSms.Size = new Size(55, 23);
            BtSms.TabIndex = 4;
            BtSms.Text = "发送";
            BtSms.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(122, 124);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // Email
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button2);
            Controls.Add(BtSms);
            Controls.Add(TbSms);
            Controls.Add(LtSms);
            Controls.Add(TbEmail);
            Controls.Add(LtEmail);
            Name = "Email";
            Size = new Size(200, 150);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LtEmail;
        private TextBox TbEmail;
        private Label LtSms;
        private TextBox TbSms;
        private Button BtSms;
        private Button button2;
    }
}
