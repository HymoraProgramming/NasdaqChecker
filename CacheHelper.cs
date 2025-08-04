using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NasdaqChecker
{
    public static class CacheHelper
    {
        public static async Task<T?> LoadFromCacheAsync<T>(string path, TimeSpan validFor, Action<DateTime>? setTimestamp = null)
        {
            if (!File.Exists(path)) return default;

            var json = await File.ReadAllTextAsync(path);

            var wrapper = JsonSerializer.Deserialize<CachedData<T>>(json);
            //var fileInfo = new FileInfo(path);

            //if(DateTime.Now - fileInfo.LastWriteTime > validFor) return default;

            if (wrapper == null || DateTime.Now - wrapper.Timestamp > validFor) return default;

            setTimestamp?.Invoke(wrapper.Timestamp);

            return wrapper.Data;

        }

        public static async Task SaveToCacheAsync<T>(string path, T data)
        {
            var wrapper = new CachedData<T>
            {
                Data = data,
                Timestamp = DateTime.Now
            };

            var json = JsonSerializer.Serialize(wrapper);
            await File.WriteAllTextAsync(path, json);
        }

        //public static DateTime GetCacheTimestamp(string path)
        //{
        //    return File.Exists(path) ? File.GetLastWriteTime(path) : DateTime.MinValue;
        //}
    }

    public class CachedData<T>
    {
        public T Data { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
