using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


    public enum Genre {};
   public class ServerData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Genre Genre { get; set; }
        public string DateAdded { get; set; }
        public string WhoAdded { get; set; }
    }
   public static class Database
    {
        public static List<ServerData> servers = new List<ServerData>();
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
                  servers.Add( JsonConvert.DeserializeObject<ServerData>(File.ReadAllText($"Servers/{server}")));      
            }
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
        public static  bool RemoveServer()
        {
            return false;
        }
       

}
