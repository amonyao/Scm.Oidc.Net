using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;
using Com.Scm.Oidc.Wpf;
using System.Diagnostics;
using System.Windows;

namespace Com.Scm
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : HandyControl.Controls.Window
    {
        private OidcConfig _Config;
        private OidcClient _Client;

        public Login()
        {
            InitializeComponent();

            Init();
        }

        public async void Init()
        {
            TbYear.Text = DateTime.Now.Year.ToString();

            _Config = new OidcConfig();
            // 使用演示应用
            _Config.UseDemo();
            // 此处也可以修改为您自己的应用
            //_Config.AppKey = "08dd257b536dd96c";
            //_Config.AppSecret = "ngkeeptx9hwjwrm8ivsqrq6ic59h0ebs";
            _Config.Prepare();

            _Client = new OidcClient(_Config);

            UcOAuth.Init(this, _Client);
            await UcVCode.Init(this, _Client);
        }

        /// <summary>
        /// 访问网站事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HlSite_Click(object sender, RoutedEventArgs e)
        {
            Browse("http://www.oidc.org.cn");
        }

        /// <summary>
        /// 显示验证登录
        /// </summary>
        public void ShowVCode()
        {
            UcVCode.Visibility = Visibility.Visible;
            UcOAuth.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 显示授权登录
        /// </summary>
        public void ShowOAuth(OidcOspInfo ospInfo)
        {
            UcVCode.Visibility = Visibility.Collapsed;
            UcOAuth.Visibility = Visibility.Visible;

            UcOAuth.Login(ospInfo);
        }

        public void ShowUser(OidcUserInfo user)
        {
            new UserInfo().ShowInfo(user);
            Close();
        }

        /// <summary>
        /// 使用系统默认浏览器，访问指定地址
        /// </summary>
        /// <param name="url"></param>
        public void Browse(string url, bool native = true)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            if (!native)
            {
                var browser = new Browser();
                browser.Owner = this;
                browser.Open(url);
                return;
            }

            try
            {
                Process.Start("explorer.exe", '"' + url + '"');
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
