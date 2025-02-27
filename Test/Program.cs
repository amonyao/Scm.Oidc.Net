using Com.Scm.Oidc;

internal class Program
{
    private static void Main(string[] args)
    {
        new Program().Test().Wait();
    }

    public async Task Test()
    {
        var _Config = new OidcConfig();
        // 使用演示应用
        _Config.UseDemo();

        var _Client = new OidcClient(_Config);
        var response = await _Client.HandshakeAsync("123");
        var ticket = response.Ticket;
        Console.WriteLine(ticket.Code);
        Console.WriteLine(ticket.Salt);

        var max = 60;
        while (max > 0)
        {
            var responses = await _Client.ListenAsync(ticket);
            if (!responses.IsSuccess())
            {
                Console.WriteLine(responses.GetMessage());
            }
            else
            {
                Console.WriteLine(responses.Ticket.Handle);
            }
            //ticket.Salt = responses.Salt;
            Thread.Sleep(1000);

            max--;
        }
    }
}