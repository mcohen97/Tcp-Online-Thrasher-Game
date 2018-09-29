using System;
using System.IO;
using Logic;
using Protocol;

namespace Services
{
    internal class ImageManager
    {
        internal void StoreImageStreaming(IConnection sender,string nickname, Package firstPart)
        {

            string directory = "../../Avatars";
            Directory.CreateDirectory(directory);
            string path = directory + "/" + nickname+ ".jpg";

            Package currentFragment = firstPart;

            using (FileStream fs = File.Create(path))
            {

                //byte[] buffer = new byte[Package.DATA_SIZE_MAX];

                while (IsImgPackage(currentFragment) && IsMaxSize(currentFragment))
                {
                    fs.Write(currentFragment.Data, 0, currentFragment.DataLength());
                    currentFragment = sender.WaitForMessage();
                }

                if (IsImgPackage(currentFragment)) {
                    fs.Write(currentFragment.Data, 0, currentFragment.DataLength());
                }
            }
        }

        private bool IsImgPackage(Package aPackage) {
            return aPackage.Command().Equals(CommandType.IMG_JPG);
        }

        private bool IsMaxSize(Package aPackage) {
            return aPackage.DataLength() == Package.MESSAGE_SIZE_MAX;
        }
    }
}