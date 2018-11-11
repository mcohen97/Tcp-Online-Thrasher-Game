using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ImageManager : IImageManager
    {
        private string directory;
        private string imageExtension;

        public ImageManager() {
            var settings = new AppSettingsReader();
            directory = (string)settings.GetValue("AvatarsFolderPath", typeof(string));
            directory += "/";
            imageExtension = ".jpg";
            Directory.CreateDirectory(directory);
        }

        public byte[] ReadImage(string path)
        {
           return File.ReadAllBytes(path);
        }

        public string SaveImage(byte[] blob, string imageName)
        {
            string fullPath = directory + imageName + imageExtension;
            using (FileStream fs = File.Create(fullPath))
            {
                fs.Write(blob, 0, blob.Length);
            }
            return fullPath;
        }
    }
}
