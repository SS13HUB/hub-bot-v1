using Discord.WebSocket;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


public enum Genre
{
    Classic = 1,
    Shooter,
    WorldWar,
    Other_Setting,
    RIP,
    SS_related,
    Remake,
    RP_orientated
};
public class UserLogs
{
    public UserLogs(SocketMessage msg)
    {
        Message = msg.Content.ToString();
        Date = msg.Timestamp.ToString();
    }
    public string Message { get; set; }
    public string Date { get; set; }
}
public class User
{
    public User(SocketMessage name)
    {
        Logs = new List<UserLogs>();
        ProposedServers = new List<ServerData>();
        Name = name.Author.ToString();
    }
    public string Name { get; set; }
    public List<UserLogs> Logs { get; set; }
    public List<ServerData> ProposedServers { get; set; }
}
public class ServerData
{
    public ServerData()
    {
        Genres = new List<Genre>();
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public List<Genre> Genres{ get; set; }
    public string DateAdded { get; set; }
    public string WhoAdded { get; set; }
}
public static class Database
{
    public static List<ServerData> servers = new List<ServerData>();
    public static List<ServerData> serversInVote = new List<ServerData>();
    public static List<ServerData> serversInAdding = new List<ServerData>();
    public static void Save()
    {
        foreach (var server in servers)
        {
            File.WriteAllText($"Servers/{server.Name}.json", JsonConvert.SerializeObject(server, Formatting.Indented)); 
        }
        foreach (var server in servers)
        {
            File.WriteAllText($"Servers on voting/{server.Name}.json", JsonConvert.SerializeObject(server, Formatting.Indented));
        }
    }
    public static void SaveLogs(SocketMessage msg)
    {
        User user = new User(msg);
        user.Logs.Add(new UserLogs(msg));   
        if (serversInAdding.Count > 0)
        {
            foreach (var serv in serversInAdding)
            {
                if (serv.WhoAdded == msg.Author.ToString())
                {
                    user.ProposedServers.Add(serv);
                }
            }
        }
        Console.WriteLine("LogSaved "+msg.Author);
        File.WriteAllText($"Logs/{msg.Author}.json", JsonConvert.SerializeObject(user, Formatting.Indented));
    }
    public static void Load()
    {
        if(servers.Count > 0 || serversInVote.Count > 0)
        {
            Save();
        }
        
        servers = new List<ServerData>();
        serversInVote = new List<ServerData>();
        foreach (var server in Directory.GetFiles("Servers"))
        {
            servers.Add(JsonConvert.DeserializeObject<ServerData>(File.ReadAllText($"Servers/{server}.json")));
        }
        foreach (var server in Directory.GetFiles("Servers on voting"))
        {
            serversInVote.Add(JsonConvert.DeserializeObject<ServerData>(File.ReadAllText($"Servers on voting/{server}.json")));
        }
    }
    public static bool HasAlready(string name)
    {
        foreach (var server in servers)
        {
            if(String.Compare(server.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
        }
        foreach (var server in serversInVote)
        {
            if (String.Compare(server.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
        }
        return false;
    }
    public static bool HasAlready(string invite, bool ordinal = false)
    {
        foreach (var server in servers)
        {
            if (String.Compare(server.Link, invite, StringComparison.Ordinal) == 0)
            {
                return true;
            }
        }
        foreach (var server in serversInVote)
        {
            if (String.Compare(server.Link, invite, StringComparison.Ordinal) == 0)
            {
                return true;
            }
        }
        return false;
    }
    public static bool HasAlready(string invite,string name)
    {
        if (HasAlready(invite) || HasAlready(name, false))
        {
            return true;
        }
        return false;
    }
    public static void ShowServerInConsole(ServerData serv)
    {
        Console.WriteLine(serv.Name);
        Console.WriteLine(serv.Description);
        Console.WriteLine(serv.Link);
        if (serv.Genres.Count > 0)
        {
            foreach (var genre in serv.Genres)
            {
                Console.Write(genre.ToString() + " ");
            }
        }
        Console.WriteLine(serv.DateAdded);
        Console.WriteLine(serv.WhoAdded);
    }
    public static string AddNewServer()
    {
        return "error";
    }
    public static string UpdateServers()
    {
        Load();
        return "Сервера обновлены";
    }
    public static bool RemoveServer()
    {
        return false;
    }


}
