using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;
using Com.Scm.Response;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Com.Scm.Uc
{
    /// <summary>
    /// UcOAuth.xaml 的交互逻辑
    /// </summary>
    public partial class UcOAuth : UserControl
    {
        private Login _Owner;
        private OidcClient _Client;

        private TicketInfo _Ticket;
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

            var response = await _Client.HandshakeAsync("login");
            if (response == null)
            {
                ShowNotice("");
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
                var response = await _Client.GetListen(_Ticket);
                if (response == null)
                {
                    ShowNotice("服务访问异常！");
                    break;
                }
                if (!response.IsSuccess())
                {
                    ShowNotice(response.GetMessage());
                    break;
                }

                _Ticket = response.Ticket;

                var ticket = response.Ticket;
                if (ticket.Handle == ListenHandle.None)
                {
                    //ShowNotice("等待用户授权");
                    break;
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
                    break;
                }

                if (ticket.Result == ListenResult.Success)
                {
                    ShowNotice("用户授权成功");
                    ShowUserInfo(response.User);
                    break;
                }

                Thread.Sleep(1000);
            }
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

        private void BtReturn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _MaxTime = 0;
            _Owner.ShowVCode();
        }
    }
}
