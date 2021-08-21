using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;

namespace Tech_Priest_Librarian
{
    class Config
    {
        public string Token { get; set; }
    }
    class Program
    {
        DiscordSocketClient client;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandHandler; 
            client.Log += Log;
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Directory.GetCurrentDirectory().ToString() + "/cfg.json")) ;
            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();
            Console.ReadLine();
        }
        //public string DataFromPath(string path)
        //{
        //    try
        //    {
        //        string rawJson;
        //        using (var reader = new StreamReader(path))
        //        {
        //            rawJson = reader.ReadToEnd();
        //        }

        //        return JsonConvert.DeserializeObject<string>(rawJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return default(string);
        //    }
        //}

        private Task CommandHandler(SocketMessage message)
        {
            if (!message.Author.IsBot && !message.Author.IsWebhook)
            {
                switch (message.Content)
                {
                    case "/Update servers":
                        message.Channel.SendMessageAsync(Database.UpdateServers());
                            break;
                    case "/Add server":
                        if (message.Author.PublicFlags.ToString().Contains("Librarian")) 
                        {
                            var serv = new ServerData();
                            message.Channel.SendMessageAsync("Введите ссылку на сервер");
                        }
                        
                        break;
                }
            }
            
            return Task.CompletedTask;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
