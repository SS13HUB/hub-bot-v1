using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

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
            client.MessageReceived += MessageHandler;
            client.Log += Log;
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Directory.GetCurrentDirectory().ToString() + "/cfg.json"));
            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();
            Console.ReadLine();
        }

        private Task MessageHandler(SocketMessage message)
        {
            if (!message.Author.IsBot && !message.Author.IsWebhook)
            {
                Commands(message);
                if (message.Channel.GetType() == typeof(SocketDMChannel))
                {
                    foreach (var serv in Database.serversInAdding)
                    {
                        if (serv.WhoAdded == message.Author.ToString())
                        {
                            AddingServer(message, serv);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
        private Task Commands(SocketMessage message)
        {
            switch (message.Content)
            {
                case "/debug":
                    Database.SaveLogs(message);
                    break;

                case "/reset":

                    if (message.Channel.GetType() == typeof(SocketDMChannel))
                    {
                        foreach (var server in Database.serversInAdding)
                        {
                            if (server.WhoAdded == message.Author.ToString())
                            {
                                Database.serversInAdding.Remove(server);
                            }
                        }

                    }
                    Database.SaveLogs(message);
                    break;

                case "/Update servers":


                    message.Channel.SendMessageAsync(Database.UpdateServers());
  
                    break;
                case "/Add server":

                    var serv = new ServerData();
                    serv.DateAdded = message.Timestamp.ToString();
                    serv.WhoAdded = message.Author.ToString();
                    message.Author.SendMessageAsync("`Введите название сервера`");
                    Database.serversInAdding.Add(serv);
                    Database.SaveLogs(message);

                    break;
            }
            return Task.CompletedTask;
        }
        private Task AddingServer(SocketMessage message, ServerData serv)
        {
            if (serv.Name == null)
            {
                if (message.Channel.GetType() == typeof(SocketDMChannel)) serv.Name = message.Content;
            }

            else if (serv.Description == null)
            {
                message.Author.SendMessageAsync("`Напишите описание к серверу`");
                if (message.Channel.GetType() == typeof(SocketDMChannel)) serv.Description = message.Content;
            }

            else if (serv.Link == null)
            {
                message.Author.SendMessageAsync("`Введите ссылку на сервер (ссылка должна быть не созданная вами, а постоянная)`");
                if (message.Content.Contains("discord.gg/") || message.Content.Contains("discordapp.com/invite/"))
                {
                    if (message.Channel.GetType() == typeof(SocketDMChannel)) serv.Link = message.Content;
                }
                else
                {
                    if (message.Channel.GetType() == typeof(SocketDMChannel)) message.Author.SendMessageAsync("`Введите корректную ссылку. /reset чтобы начать сначала`");
                }
            }

            else if (serv.Genres.Count <= 0)
            {
                string msg = "`Ввыберите номер жанра, или несколько через запятую (1,2)`";
                foreach (var genre in Enum.GetValues(typeof(Genre)))
                {
                    msg += $"\n {genre}";
                }
                message.Author.SendMessageAsync(msg);
                //serv.Genres = message.Content
               
            }
            Database.SaveLogs(message);
            return Task.CompletedTask;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
