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
public class ServerData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public List<Genre> Genres { get; set; }
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
            File.WriteAllText($"Servers/{server.Name}", JsonConvert.SerializeObject(server));
        }
    }
    public static void Load()
    {
        foreach (var server in Directory.GetFiles("Servers"))
        {
            servers.Add(JsonConvert.DeserializeObject<ServerData>(File.ReadAllText($"Servers/{server}")));
        }
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
