using System;
using System.IO;
using UsersLogic;
using Protocol;
using System.Configuration;

namespace Services
{
    internal class ImageManager
    {
        internal string StoreImageStreaming(IConnection sender,string nickname, Package firstPart)
        {
            var settings = new AppSettingsReader();
            string directory = (string)settings.GetValue("AvatarsFolderPath", typeof(string));
            Directory.CreateDirectory(directory);
            string path = directory + "/" + nickname+ ".jpg";

            Package currentFragment = firstPart;

            using (FileStream fs = File.Create(path))
            {

                while (IsImgPackage(currentFragment) && IsMaxSize(currentFragment))
                {
                    fs.Write(currentFragment.Data, 0, currentFragment.DataLength());
                    currentFragment = sender.WaitForMessage();
                }

                if (IsImgPackage(currentFragment)) {
                    fs.Write(currentFragment.Data, 0, currentFragment.DataLength());
                }
            }
            return path;
        }

        private bool IsImgPackage(Package aPackage) {
            return aPackage.Command().Equals(CommandType.IMG_JPG);
        }

        private bool IsMaxSize(Package aPackage) {
            return aPackage.DataLength() == Package.MESSAGE_SIZE_MAX;
        }
    }
}