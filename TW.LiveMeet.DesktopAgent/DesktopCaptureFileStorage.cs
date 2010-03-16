using System;
using System.Drawing;
using System.IO;
using TW.Coder;

namespace TW.LiveMeet.DesktopAgent
{
    public class DesktopCaptureFileStorage : IDesktopCaptureStorage
    {
        private readonly string filePath;
        private readonly string folderName;
        private readonly string baseFileName;
        private int fileIndex;

        public DesktopCaptureFileStorage(string filePath)
        {
            this.filePath = filePath;
            folderName = Guid.NewGuid().ToString();

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            baseFileName = "DesktopImage";
        }

        public void Process(RgbFrame frame)
        {
            Image snapShot = RgbFrameFactory.CreateBitmap((RgbFrame) frame);
            snapShot.Save(FolderPath + baseFileName + "_" + fileIndex + ".jpg");
            fileIndex++;
        }

        public string FolderPath
        {
            get { return filePath + @"\" + folderName + @"\"; }
        }
    }
}