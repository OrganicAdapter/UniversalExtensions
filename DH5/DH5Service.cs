using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalExtensions.DH5
{
    public delegate void ExtractionCompletedEventHandler(object sender, List<DH5Page> pages);

    public class DH5Service
    {
        public static async Task<List<DH5Page>> Extract(string path)
        {
            var list = new List<DH5Page>();

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                var dh5 = await reader.ReadToEndAsync();
                var split = dh5.Split(new string[] { ";EH;" }, StringSplitOptions.None);
                var content = split[1];
                var contentbytes = Encoding.UTF8.GetBytes(content);
                var bytes = Convert.FromBase64String(split[0]);
                var header = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                var items = header.Split(';');
                items = items.Take(items.Length - 1).ToArray();

                foreach (var item in items)
                {
                    var info = item.Split(',');

                    var start = int.Parse(info[0]);
                    var length = int.Parse(info[1]);

                    bytes = contentbytes.Skip(start).Take(length).ToArray();
                    var current = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    
                    list.Add(new DH5Page() { Page = current, Path = info[2] });
                }
            }

            return list;
        }
    }
}
