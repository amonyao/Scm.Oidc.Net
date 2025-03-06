using Com.Scm.Oidc;

internal class Program
{
    private static void Main(string[] args)
    {
        new Program().Test().Wait();
    }

    public async Task Test()
    {
        var config = new OidcConfig();
        config.UseTest();
        var client = new OidcClient(config);
        var response = await client.HandshakeAsync("1234");
        var ticket = response.Ticket;
        var qty = 30;
        while (qty > 0)
        {
            Thread.Sleep(1000);
            var kk = await client.ListenAsync(ticket);
            ticket.Salt = kk.Salt;
            Console.WriteLine(kk.Salt);
            qty -= 1;
        }
    }
}