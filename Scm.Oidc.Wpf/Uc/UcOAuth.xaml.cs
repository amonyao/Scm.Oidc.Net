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

        public void Login(OidcOspInfo ospInfo)
        {
            _OspInfo = ospInfo;

            PbLogo.Source = new BitmapImage(new Uri(ospInfo.GetIconUrl()));
            _Owner.Browse(_Client.GetLoginUrl(ospInfo.Code));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request_id"></param>
        /// <returns></returns>
        private async Task<bool> GetTicket(string request_id)
        {
            var response = await _Client.HandshakeAsync(request_id);
            if (response == null || !response.IsSuccess())
            {
                return false;
            }

            _Ticket = response.Ticket;
            return true;
        }

        private void GetListen(string seq)
        {
            Task.Run(DddAsync);
        }

        private async Task DddAsync()
        {
            var response = await _Client.GetListen(_Ticket);
            if (response == null || !response.IsSuccess())
            {
                return;
            }

            var ticket = response.Ticket;
            if (ticket.Handle == ListenHandle.None)
            {
                //ShowNotice("等待用户授权");
                return;
            }

            if (ticket.Handle == ListenHandle.Todo)
            {
                ShowNotice("等待用户授权");
                return;
            }

            if (ticket.Handle == ListenHandle.Doing)
            {
                ShowNotice("用户授权中…");
                return;
            }

            if (ticket.Result == ListenResult.Failure)
            {
                ShowNotice("用户授权失败");
                return;
            }

            if (ticket.Result == ListenResult.Success)
            {
                ShowNotice("用户授权成功");
                return;
            }
        }

        private void ShowNotice(string message)
        {
            TbNotice.Text = message;
        }

        private void BtReturn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _Owner.ShowVCode();
        }
    }
}
