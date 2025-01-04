﻿using Com.Scm.Oidc.Response;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Com.Scm.Oidc.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcUserInfo _User;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowInfo(OidcUserInfo userInfo)
        {
            Show();

            if (userInfo == null)
            {
                return;
            }

            _User = userInfo;

            PbIcon.Source = new BitmapImage(new Uri(userInfo.GetAvatarUrl()));
            TbInfo.Text = "您好：" + userInfo.Name;
        }
    }
}