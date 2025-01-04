using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;
using Com.Scm.Oidc.Wpf;
using Com.Scm.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Com.Scm
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private OidcClient _Client;
        private OidcSmsEnums _Type = OidcSmsEnums.Email;
        private string _Key;
        private string _Code;

        public Login()
        {
            InitializeComponent();

            Init();
        }

        public async void Init()
        {
            TbYear.Text = DateTime.Now.Year.ToString();

            var config = new OidcConfig();
            config.AppKey = "08dd257b536dd96c";
            config.AppSecret = "ngkeeptx9hwjwrm8ivsqrq6ic59h0ebs";
            //config.LoadDefault();

            _Client = new OidcClient(config);

            var ospList = await _Client.ListAppOspAsync();
            foreach (var osp in ospList)
            {
                if (!osp.IsOAuth())
                {
                    continue;
                }

                var button = new Button();
                button.Margin = new Thickness(2);
                button.Padding = new Thickness(2);
                button.Width = 28;
                button.Height = 28;
                button.Content = new Image()
                {
                    Source = new BitmapImage(new Uri(osp.GetIconUrl()))
                };
                button.ToolTip = $"使用 {osp.Name} 登录";
                button.Tag = osp;
                button.Click += BtOAuth_Click;
                SpOAuth.Children.Add(button);
            }
        }

        private async void BtOAuth_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var osp = button.Tag as OidcOspInfo;
            if (osp == null)
            {
                return;
            }

            var response = await _Client.LoginAsync(osp.Code, "login");
            if (response == null)
            {
                return;
            }
        }

        /// <summary>
        /// 切换手机号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbPhone_Click(object sender, RoutedEventArgs e)
        {
            _Type = OidcSmsEnums.Phone;
            EgPhone.Visibility = Visibility.Visible;
            EgEmail.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 切换电子邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbEmail_Click(object sender, RoutedEventArgs e)
        {
            _Type = OidcSmsEnums.Email;
            EgPhone.Visibility = Visibility.Collapsed;
            EgEmail.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtSend_Click(object sender, RoutedEventArgs e)
        {
            if (_Type == OidcSmsEnums.Phone)
            {
                await SendPhone();
                return;
            }

            if (_Type == OidcSmsEnums.Email)
            {
                await SendEmail();
                return;
            }
        }

        private async Task SendPhone()
        {
            var phone = TbPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                ShowNotice("请输入手机号码！");
                return;
            }

            if (!TextUtils.IsPhone(phone))
            {
                ShowNotice("无效的手机号码格式！");
                return;
            }

            await SendSms(phone);
        }

        private async Task SendEmail()
        {
            var email = TbEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                ShowNotice("请输入电子邮件！");
                return;
            }

            if (!TextUtils.IsMail(email))
            {
                ShowNotice("无效的电子邮件格式！");
                return;
            }

            await SendSms(email);
        }

        private async Task SendSms(string code)
        {
            BtSend.IsEnabled = false;

            _Code = code;
            _Key = UuidString();

            var response = await _Client.SendSmsAsync(_Type, code, _Key);
            if (response == null)
            {
                BtSend.IsEnabled = true;
                ShowNotice("服务访问异常，请稍后重试！");
                return;
            }
            if (!response.IsSuccess())
            {
                BtSend.IsEnabled = true;
                ShowNotice(response.GetMessage());
                return;
            }

            CountDown(BtSend);
        }

        private async void BtVerify_Click(object sender, RoutedEventArgs e)
        {
            var sms = TbSms.Text.Trim();
            if (string.IsNullOrWhiteSpace(sms))
            {
                ShowNotice("请输入验证码！");
                return;
            }

            if (!Regex.IsMatch(sms, @"^\d{6}$"))
            {
                ShowNotice("无效的验证码格式！");
                return;
            }

            BtVerify.IsEnabled = false;
            var response = await _Client.VerifySmsAsync(_Type, _Code, _Key, sms);
            if (response == null)
            {
                BtVerify.IsEnabled = true;
                ShowNotice("服务访问异常，请稍后重试！");
                return;
            }
            if (!response.IsSuccess())
            {
                BtVerify.IsEnabled = true;
                ShowNotice(response.GetMessage());
                return;
            }

            new MainWindow().ShowInfo(response.User);
            Close();
        }

        private void ShowNotice(string message)
        {
            TbNotice.Text = message;
        }

        private void CountDown(Button button, int step = 60)
        {
            if (button == null || step < 1)
            {
                return;
            }

            Task.Run(new Action(() =>
            {
                while (step > 0)
                {
                    ShowContent(button, $"重新发送({step})");
                    step -= 1;
                    Thread.Sleep(1000);
                }

                ChangeEnabled(button, true);
                ShowContent(button, $"重新发送");
            }));
        }

        private void ChangeEnabled(Button button, bool enabled)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                button.IsEnabled = enabled;
            });
        }

        private void ShowContent(Button button, object content)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                button.Content = content;
            });
        }

        private string UuidString()
        {
            return TextUtils.ToHexString(Guid.NewGuid().ToByteArray());
        }

        private void HlSite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", "http://www.oidc.org.cn");
            }
            catch
            {
            }
        }
    }
}
