namespace Scm.OIdc.Client
{
    public class OidcConfig
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUrl { get; set; }
        /// <summary>
        /// 应用模式
        /// </summary>
        public int Mode { get; set; }

        public void Prepare()
        {
        }

        public void LoadDefault()
        {
            AppKey = "0";
            AppSecret = "e2t22gcqr5wf311hnopxyqylpdwx6uhp";
        }
    }
}
