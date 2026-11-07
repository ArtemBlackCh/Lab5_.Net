using System.IO;
using System.Text.Json;

namespace laba5
{
    public class JsonFileReader : IFileLoader
    {
        public async Task<List<T>?> LoadAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
