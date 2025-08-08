using System.Drawing;
using System.Windows.Forms;
using System.IO;

public class Popup
{
    public static void ShowScreenshotPopup(Bitmap screenshot, string fileName)
    {
        string userInput = "";

        // Get the path where the screenshot was saved
        string directoryName = "Screenshots 2";
        string picturesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
        string directoryPath = Path.Combine(picturesPath, directoryName);
        string imagePath = Path.Combine(directoryPath, fileName);

        double aspectRatio = (double)screenshot.Width / screenshot.Height;
        int extraHeight = 40 + 50; // TextBox + Button heights (from layout)
        int minImgWidth = 200;
        int minImgHeight = (int)(minImgWidth / aspectRatio);
        int minHeight = minImgHeight + extraHeight;

        Form popup = new Form();
        popup.Text = $"Screenshot Preview - {fileName}";
        popup.StartPosition = FormStartPosition.CenterScreen;
        popup.Size = new Size(screenshot.Width / 2, screenshot.Height / 2 + extraHeight);
        popup.FormBorderStyle = FormBorderStyle.Sizable;
        popup.MaximizeBox = true;
        popup.MinimizeBox = true;
        popup.BackColor = Color.FromArgb(220, 235, 250); // Calm light blue

        popup.MinimumSize = new Size(minImgWidth, minHeight);

        TableLayoutPanel layout = new TableLayoutPanel();
        layout.Dock = DockStyle.Fill;
        layout.RowCount = 3;
        layout.ColumnCount = 1;
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F)); // PictureBox takes most space
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        layout.BackColor = Color.Transparent;

        PictureBox pictureBox = new PictureBox();
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.Image = (Bitmap)screenshot.Clone(); // Use original size for quality
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox.BackColor = Color.WhiteSmoke;

        TextBox textBox = new TextBox();
        textBox.Dock = DockStyle.Fill;
        textBox.PlaceholderText = "Add a note to your screenshot...";
        textBox.Font = new Font("Segoe UI", 12, FontStyle.Italic);
        textBox.BackColor = Color.FromArgb(240, 248, 255);
        textBox.ForeColor = Color.FromArgb(60, 60, 80);
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Margin = new Padding(20, 8, 20, 8);

        Button submitButton = new Button();
        submitButton.Text = "Submit";
        submitButton.Dock = DockStyle.Fill;
        submitButton.Height = 40;
        submitButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        submitButton.BackColor = Color.FromArgb(180, 210, 245);
        submitButton.ForeColor = Color.FromArgb(30, 50, 80);
        submitButton.FlatStyle = FlatStyle.Flat;
        submitButton.FlatAppearance.BorderSize = 0;
        submitButton.Margin = new Padding(100, 5, 100, 15);
        submitButton.Cursor = Cursors.Hand;

        layout.Controls.Add(pictureBox, 0, 0);
        layout.Controls.Add(textBox, 0, 1);
        layout.Controls.Add(submitButton, 0, 2);

        popup.Controls.Add(layout);

        // Enforce resizing in image aspect ratio
        bool resizing = false;
        popup.Resize += (s, e) =>
        {
            if (resizing) return;
            resizing = true;

            int totalExtra = extraHeight + popup.Height - popup.ClientSize.Height;
            int clientHeight = popup.ClientSize.Height;
            int clientWidth = popup.ClientSize.Width;

            // Calculate new width and height to maintain aspect ratio
            int newImgHeight = clientHeight - extraHeight;
            int newImgWidth = (int)(newImgHeight * aspectRatio);

            if (clientWidth != newImgWidth)
            {
                newImgWidth = clientWidth;
                newImgHeight = (int)(newImgWidth / aspectRatio);
            }

            popup.ClientSize = new Size(newImgWidth, newImgHeight + extraHeight);

            resizing = false;
        };

        submitButton.Click += (s, e) =>
        {
            userInput = textBox.Text;
            // For now it only posts your screenshots to my specific webhook on my server
            // I want to make it so there's a selection screen on the popup where you can choose which server and channel you can post your screenshots to
            _ = Discord.SendTextAndImageToDiscordAsync(
                "https://discord.com/api/webhooks/1403375327387848765/jNaJSM7e4TQVBFl9KqOaNtz8mqv6bZHMqMCBOgrbwrlYT1OLyxViQNEf_Tw5N-9yTJsa",
                userInput,
                imagePath
            );
            popup.Close();
        };

        popup.ShowDialog();
    }
}