using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;

namespace video_downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string direcotryPath = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            formatList.Items.Add("MP4 - VIDEO");
            formatList.Items.Add("MP3 - AUDIO");

            if (Properties.Settings.Default.isSaved == 1)
            {
                formatList.SelectedIndex = Properties.Settings.Default.savedFormat;
                direcotryPath = Properties.Settings.Default.savedOutput;
            }
            else
            {
                formatList.SelectedIndex = 0;
            }
        }

        private void outputButton_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
            direcotryPath = fbd.SelectedPath;
            outputLabel.Text = direcotryPath;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (formatList.SelectedItem == null)
            {
                MessageBox.Show("Nincs kiv�lasztva form�tum");
                return;
            }
            if (direcotryPath == "")
            {
                MessageBox.Show("Nincs kiv�lasztva output (c�lk�nyvt�r)");
                return;
            }
            if (linkBox.Text == "")
            {
                MessageBox.Show("Nincs megadva link");
                return;
            }

            try
            {
                var youTube = YouTube.Default;
                var video = youTube.GetVideo(linkBox.Text);
                var inputFile = new MediaFile { Filename = direcotryPath + @"\" + video.FullName };
                var outputFile = new MediaFile { Filename = direcotryPath + @"\" + video.FullName.Remove(video.FullName.Length - 4) + ".mp3" };

                if (formatList.SelectedIndex == 1)
                {
                    File.WriteAllBytes(direcotryPath + @"\" + video.FullName + " (video)", video.GetBytes());
                    using var engine = new Engine();
                    engine.Convert(inputFile, outputFile);
                    File.Delete(direcotryPath + @"\" + video.FullName);
                }
                else
                {
                    File.WriteAllBytes(direcotryPath + @"\" + video.FullName, video.GetBytes());
                }

                MessageBox.Show("Done");
            }

            catch (Exception)
            {
                MessageBox.Show("Nem j� a link");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.savedFormat = formatList.SelectedIndex;
            Properties.Settings.Default.savedOutput = direcotryPath;
            Properties.Settings.Default.isSaved = 1;
        }
    }
}