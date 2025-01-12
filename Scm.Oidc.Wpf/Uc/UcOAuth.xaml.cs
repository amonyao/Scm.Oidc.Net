using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;
using Com.Scm.Response;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Com.Scm.Uc
{
    /// <summary>
    /// 三方授权登录
    /// </summary>
    public partial class UcOAuth : UserControl
    {
        /// <summary>
        /// 父窗体
        /// </summary>
        private Login _Owner;
        /// <summary>
        /// OIDC客户端
        /// </summary>
        private OidcClient _Client;
        /// <summary>
        /// 客户端凭据
        /// </summary>
        private TicketInfo _Ticket;
        /// <summary>
        /// 服务商信息
        /// </summary>
        private OidcOspInfo _OspInfo;

        public UcOAuth()
        {
            InitializeComponent();
        }

        public void Init(Login owner, OidcClient client)
        {
            _Owner = owner;
            _Client = client;
        }

        public async void Login(OidcOspInfo ospInfo)
        {
            _OspInfo = ospInfo;

            PbLogo.Source = new BitmapImage(new Uri(ospInfo.GetIconUrl()));
            LcLoading.Visibility = System.Windows.Visibility.Visible;

            var response = await _Client.HandshakeAsync("login");
            if (response == null)
            {
                ShowNotice("服务端通讯异常！");
                return;
            }
            if (!response.IsSuccess())
            {
                ShowNotice(response.GetMessage());
                return;
            }

            _Ticket = response.Ticket;
            _Owner.Browse(_Client.GetOAuthUrl(ospInfo, _Ticket.Code));
            Listen();
        }

        private void Listen()
        {
            _MaxTime = 60 * 10;// 等待10分钟
            Task.Run(ListenAsync);
        }

        private int _MaxTime = 0;
        private async Task ListenAsync()
        {
            while (_MaxTime > 0)
            {
                _MaxTime -= 1;

                var response = await _Client.GetListen(_Ticket);
                if (response == null)
                {
                    ShowNotice("服务访问异常！");
                    return;
                }
                if (!response.IsSuccess())
                {
                    ShowNotice(response.GetMessage());
                    return;
                }

                _Ticket = response.Ticket;

                var ticket = response.Ticket;
                if (ticket.Handle == ListenHandle.None)
                {
                    //ShowNotice("等待用户授权");
                    return;
                }

                if (ticket.Handle == ListenHandle.Todo)
                {
                    ShowNotice("等待用户授权");
                    continue;
                }

                if (ticket.Handle == ListenHandle.Doing)
                {
                    ShowNotice("用户授权中…");
                    continue;
                }

                if (ticket.Result == ListenResult.Failure)
                {
                    ShowNotice("用户授权失败");
                    return;
                }

                if (ticket.Result == ListenResult.Success)
                {
                    ShowNotice("用户授权成功");
                    ShowUserInfo(response.User);
                    return;
                }

                Thread.Sleep(1000);
            }

            ShowNotice("授权超时，请返回重试！");
            LcLoading.Visibility = System.Windows.Visibility.Hidden;
        }

        private void BtReturn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _MaxTime = 0;
            _Owner.ShowVCode();
        }

        private void ShowUserInfo(OidcUserInfo user)
        {
            Dispatcher.Invoke(() =>
            {
                _Owner.ShowUser(user);
            });
        }

        private void ShowNotice(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TbNotice.Text = message;
            });
        }
    }
}
