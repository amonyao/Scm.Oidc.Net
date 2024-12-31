namespace Com.Scm.Oidc.Web
{
    public class OidcUtils
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Setup(WebApplicationBuilder builder)
        {
            Configuration = builder.Configuration;

            var envConfig = GetConfig<OidcConfig>("Oidc");
            if (envConfig == null)
            {
                envConfig = new OidcConfig();
                envConfig.LoadDefault();
            }
            envConfig.Prepare();

            var oidcClient = new OidcClient(envConfig);
            builder.Services.AddSingleton(oidcClient);
        }

        public static T GetConfig<T>(string path)
        {
            return Configuration.GetSection(path).Get<T>();
        }
    }
}
