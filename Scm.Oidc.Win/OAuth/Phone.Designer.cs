namespace Com.Scm.Oidc.Win
{
    partial class Phone
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
            LtPhone = new Label();
            TbPhone = new TextBox();
            LtSms = new Label();
            TbSms = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // LtPhone
            // 
            LtPhone.AutoSize = true;
            LtPhone.Location = new Point(3, 6);
            LtPhone.Name = "LtPhone";
            LtPhone.Size = new Size(56, 17);
            LtPhone.TabIndex = 0;
            LtPhone.Text = "手机号码";
            // 
            // TbPhone
            // 
            TbPhone.Location = new Point(65, 3);
            TbPhone.Name = "TbPhone";
            TbPhone.Size = new Size(132, 23);
            TbPhone.TabIndex = 1;
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
            // button1
            // 
            button1.FlatStyle = FlatStyle.Popup;
            button1.Location = new Point(142, 32);
            button1.Name = "button1";
            button1.Size = new Size(55, 23);
            button1.TabIndex = 4;
            button1.Text = "发送";
            button1.UseVisualStyleBackColor = true;
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
            // Phone
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(TbSms);
            Controls.Add(LtSms);
            Controls.Add(TbPhone);
            Controls.Add(LtPhone);
            Name = "Phone";
            Size = new Size(200, 150);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LtPhone;
        private TextBox TbPhone;
        private Label LtSms;
        private TextBox TbSms;
        private Button button1;
        private Button button2;
    }
}
