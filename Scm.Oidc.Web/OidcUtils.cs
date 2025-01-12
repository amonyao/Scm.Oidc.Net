namespace Com.Scm.Oidc.Web
{
    public class OidcUtils
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Setup(WebApplicationBuilder builder)
        {
            Configuration = builder.Configuration;

            var oidcConfig = GetConfig<OidcConfig>("Oidc");
            if (oidcConfig == null)
            {
                oidcConfig = new OidcConfig();
                oidcConfig.UseDemo();
            }
            oidcConfig.Prepare();

            var oidcClient = new OidcClient(oidcConfig);
            builder.Services.AddSingleton(oidcClient);
        }

        public static T GetConfig<T>(string path)
        {
            return Configuration.GetSection(path).Get<T>();
        }
    }
}
