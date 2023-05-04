using System.IO;
using Enums;

namespace Models
{
    public class DataStore
    {
        public void SaveData(string path, string data)
        {
           
            using (StreamWriter writer = new StreamWriter(path,false))
            {
                writer.Write(data);
            }
        }

        public string LoadData(string path)
        {
            string data = "";
            
            using (StreamReader reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
            }

            return data;
        }
    }
}