namespace Com.Scm.Oidc.Win
{
    partial class Login
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
            pictureBox1 = new PictureBox();
            PlOidc = new FlowLayoutPanel();
            panel2 = new Panel();
            BtLogin = new Button();
            BtSms = new Button();
            TbSms = new TextBox();
            LtSms = new Label();
            TbEmail = new TextBox();
            LtEmail = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(384, 50);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // PlOidc
            // 
            PlOidc.Dock = DockStyle.Bottom;
            PlOidc.Location = new Point(0, 211);
            PlOidc.Name = "PlOidc";
            PlOidc.Size = new Size(384, 50);
            PlOidc.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(BtLogin);
            panel2.Controls.Add(BtSms);
            panel2.Controls.Add(TbSms);
            panel2.Controls.Add(LtSms);
            panel2.Controls.Add(TbEmail);
            panel2.Controls.Add(LtEmail);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 50);
            panel2.Name = "panel2";
            panel2.Size = new Size(384, 161);
            panel2.TabIndex = 2;
            // 
            // BtLogin
            // 
            BtLogin.Location = new Point(197, 80);
            BtLogin.Name = "BtLogin";
            BtLogin.Size = new Size(75, 23);
            BtLogin.TabIndex = 5;
            BtLogin.Text = "登录";
            BtLogin.UseVisualStyleBackColor = true;
            // 
            // BtSms
            // 
            BtSms.FlatStyle = FlatStyle.Popup;
            BtSms.Location = new Point(227, 51);
            BtSms.Name = "BtSms";
            BtSms.Size = new Size(45, 23);
            BtSms.TabIndex = 4;
            BtSms.Text = "发送";
            BtSms.UseVisualStyleBackColor = true;
            // 
            // TbSms
            // 
            TbSms.Location = new Point(121, 51);
            TbSms.Name = "TbSms";
            TbSms.Size = new Size(100, 23);
            TbSms.TabIndex = 3;
            // 
            // LtSms
            // 
            LtSms.AutoSize = true;
            LtSms.Location = new Point(71, 54);
            LtSms.Name = "LtSms";
            LtSms.Size = new Size(44, 17);
            LtSms.TabIndex = 2;
            LtSms.Text = "验证码";
            // 
            // TbEmail
            // 
            TbEmail.Location = new Point(121, 22);
            TbEmail.Name = "TbEmail";
            TbEmail.Size = new Size(151, 23);
            TbEmail.TabIndex = 1;
            // 
            // LtEmail
            // 
            LtEmail.AutoSize = true;
            LtEmail.Location = new Point(83, 25);
            LtEmail.Name = "LtEmail";
            LtEmail.Size = new Size(32, 17);
            LtEmail.TabIndex = 0;
            LtEmail.Text = "邮件";
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 261);
            Controls.Add(panel2);
            Controls.Add(PlOidc);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Login";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "登录";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private FlowLayoutPanel PlOidc;
        private Panel panel2;
        private Button BtLogin;
        private Button BtSms;
        private TextBox TbSms;
        private Label LtSms;
        private TextBox TbEmail;
        private Label LtEmail;
    }
}