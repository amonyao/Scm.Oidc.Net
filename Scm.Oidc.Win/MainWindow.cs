namespace Com.Scm.Oidc.Win
{
    public partial class MainWindow : Form
    {
        private OidcClient _Client;

        public MainWindow()
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

            var list = await _Client.ListAppOspAsync();
            if (list == null)
            {
                return;
            }
            var i = list.Count;
        }
    }
}
