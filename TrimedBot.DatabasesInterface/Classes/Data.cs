using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DatabasesInterface.Classes
{
    public class Data
    {
        public readonly string filePath;

        public Data()
        {
            filePath = $"{ApplicationInfo.Path}\\Connections";
        }

        public async Task Save(object item)
        {
            if (item != null)
            {
                CheckFile();

                var serializedObject = JsonConvert.SerializeObject(item);

                using (var stream = new StreamWriter(filePath, true))
                {
                    await stream.WriteAsync(serializedObject);
                }
            }
        }

        public async Task<T> Load<T>()
        {
            CheckFile();

            Char[] buffer;
            using (var stream = new StreamReader(filePath))
            {
                buffer = new Char[(int)stream.BaseStream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.BaseStream.Length);
            }

            return JsonConvert.DeserializeObject<T>(new string(buffer));
        }

        public void CheckFile()
        {
            if (!File.Exists(filePath)) File.Create(filePath);
        }
    }
}
