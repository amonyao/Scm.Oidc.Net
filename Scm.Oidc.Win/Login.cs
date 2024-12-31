using Com.Scm.OAuth.Response;
using Scm.OIdc.Client;

namespace Scm.OAuth.Win
{
    public partial class Login : Form
    {
        private OidcClient _Client;

        public Login()
        {
            InitializeComponent();

            Init();
        }

        public async void Init()
        {
            if (_Client == null)
            {
                var config = new OidcConfig();
                config.AppKey = "08dc5d527578a465";
                config.AppSecret = "e2fpomv41f45365tf5lra76c1cnnp9ua";
                _Client = new OidcClient(config);
            }

            var list = await _Client.ListAppOsp();
            if (list == null)
            {
                return;
            }

            foreach (var item in list)
            {
                var button = new Button();
                button.FlatStyle = FlatStyle.Popup;
                button.Text = item.Name;
                button.Width = 23;
                button.Tag = item;
                button.Click += Oidc_Click;
                PlOidc.Controls.Add(button);
            }
        }

        private void Oidc_Click(object? sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var item = button.Tag as OspItem;
            if (item == null)
            {
                return;
            }

            MessageBox.Show(item.Name);
        }
    }
}
