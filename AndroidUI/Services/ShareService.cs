using System;
using System.Collections.Generic;
using System.Text;

namespace AndroidUI.Services
{
    public class ShareService
    {
        public async Task ShareImageAsync(string imageAssets, string cacheImage)
        {
            using (Stream assetStream = await FileSystem.OpenAppPackageFileAsync("wwwroot/" + imageAssets))
            {
                using (FileStream fileStream = File.Create(cacheImage))
                {
                    await assetStream.CopyToAsync(fileStream);
                }
            }

            await Share.Default.RequestAsync(new ShareFileRequest()
            {
                Title = "Share",
                File = new ShareFile(cacheImage)
            });
        }

        public async Task DownloadImageAsync(string imageAssets, string cacheImage)
        {

        }
    }
}