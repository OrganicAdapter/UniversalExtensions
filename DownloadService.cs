using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace UniversalExtensions
{
    public delegate void DownloadCompletedEventHandler(object sender, bool result);

    public class DownloadService
    {
        public event DownloadCompletedEventHandler DownloadCompletedEvent;

        public async Task DownloadFile(string uri, string local, CreationCollisionOption collisionOption = CreationCollisionOption.ReplaceExisting, string folder = null)
        {
            var path = new Uri(uri);
            var downloader = new BackgroundDownloader();

            StorageFile file = null;

            if (folder != null)
            {
                var localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);
                file = await localFolder.CreateFileAsync(local, collisionOption);
            }
            else
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(local, collisionOption);

            var download = downloader.CreateDownload(path, file);
            await StartDownloadAsync(download);
            
            if (DownloadCompletedEvent != null)
                DownloadCompletedEvent(this, true);
        }

        private async Task StartDownloadAsync(DownloadOperation download)
        {
            var progress = new Progress<DownloadOperation>();
            var a = await download.StartAsync().AsTask(progress);
        }
    }
}
