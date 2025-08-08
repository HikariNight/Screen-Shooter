using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

public class Screenshot
{
    public static void Capture()
    {
        Rectangle bounds = Screen.PrimaryScreen.Bounds;

        string directoryName = "Screenshots 2";
        string picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        string directoryPath = Path.Combine(picturesPath, directoryName);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string baseFileName = $"{DateTime.Now:yyyy-MM-dd}";
        string fileName = baseFileName + ".png";
        string path = Path.Combine(directoryPath, fileName);

        int count = 2;
        while (File.Exists(path))
        {
            fileName = $"{baseFileName} ({count}).png";
            path = Path.Combine(directoryPath, fileName);
            count++;
        }

        using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }

            bitmap.Save(path, ImageFormat.Png);
            Console.WriteLine($"Screenshot saved to: {path}");

            // Show the screenshot in a popup window, pass the filename for the title
            Popup.ShowScreenshotPopup(bitmap, fileName);
        }
    }


}

