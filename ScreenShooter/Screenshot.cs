using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO; // Add this for directory and path operations

public class Screenshot{
    public static void Capture(){
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

        // If file (taken on the same date) exists, add (2), (3), etc.
        int count = 2;
        while (File.Exists(path))
        {
            fileName = $"{baseFileName} ({count}).png";
            path = Path.Combine(directoryPath, fileName);
            count++;
        }

        // Create bitmap to hold the screenshot
        using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
        {
            // Create graphics object from the bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Capture screenshot from the top-left corner of the primary screen
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }

            // Save the screenshot to the specified directory
            bitmap.Save(path, ImageFormat.Png);
            Console.WriteLine($"Screenshot saved to: {path}");
        }
    }
}

