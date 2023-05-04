using Newtonsoft.Json;

namespace Enums
{
    public class SerializeUtil<T>
    {
        public string Serialize(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public T DeSerialize(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}