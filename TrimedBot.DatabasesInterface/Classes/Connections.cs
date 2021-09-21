using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DatabasesInterface.Classes
{
    public static class Connections
    {
        public static List<Connection> ConnectionsList { get; private set; } = new List<Connection>();

        public static void Add(Connection connection)
        {
            ConnectionsList.Add(connection);
        }

        public static void Delete(Connection connection)
        {
            ConnectionsList.Remove(connection);
        }

        public static void Clear()
        {
            ConnectionsList.Clear();
        }

        public static async Task SaveToFile()
        {
            if (ConnectionsList.Count > 0)
                await new Data().Save(ConnectionsList);
        }

        public static async Task LoadFromFile()
        {
            var data = await new Data().Load<List<Connection>>();
            if (data != null)
                ConnectionsList.AddRange(data);
        }
    }

    public class Connection
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
