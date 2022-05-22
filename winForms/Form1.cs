using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using WMPLib;
using TagLib;
using System.IO;
using System.Management;

namespace winForms
{
    public partial class Form1 : Form
    {

        WMPLib.WindowsMediaPlayer pl = new WMPLib.WindowsMediaPlayer();
        bool pause = false;
        string filePath = "";

        Timer timer1 = new Timer();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = winForms.Properties.Resources.sad_cover;
            trackBar1.Enabled = false;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);

        }

        //pause
        private void button1_Click(object sender, EventArgs e)
        {
            pl.controls.pause();
            pause = true;
            timer1.Stop();
            trackBar1.Enabled = false;

        }

        //play
        private void button2_Click(object sender, EventArgs e)
        {
            if (pause)
            {
                pl.controls.play();
                pause = false;
                timer1.Start();
                trackBar1.Enabled = true;
            }
            else
            {
                pl.controls.stop();
                //track
                label4.Text = null;
                //album
                label3.Text = null;
                //band
                label5.Text = null;
                label7.Text = "00:00";
                label8.Text = "00:00";
                pictureBox1.Image = winForms.Properties.Resources.sad_cover;

                timer1.Stop();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\Users\\" + Environment.UserName + "\\Music";
                    openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;
                        play();

                    }
                }
            }

        }

        //music play func
        private void play()
        {
            timer1.Start();
            trackBar1.Enabled = true;

            pl.URL = filePath;
            pl.controls.play();
            TagLib.File file = TagLib.File.Create(filePath);
            var mStream = new MemoryStream();
            var firstPicture = file.Tag.Pictures.FirstOrDefault();
            //track
            if (!(file.Tag.Title == null))
            {
                label4.Text = file.Tag.Title;
            }
            else { label4.Text = file.Name; }

            //album 
            if (!(file.Tag.Album == null))
            {
                label3.Text = file.Tag.Album;
            }
            else { label3.Text = "none"; }

            //band
            try
            {
                label5.Text = Convert.ToString(file.Tag.Performers[0]);
            }
            catch { label5.Text = "none"; }

            if (firstPicture != null)
            {
                byte[] pData = firstPicture.Data.Data;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                var bm = new Bitmap(mStream, false);
                mStream.Dispose();
                pictureBox1.Image = bm;
            }
            else
            {
                pictureBox1.Image = winForms.Properties.Resources.sad_cover;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            double dlina = pl.currentMedia.duration;
            trackBar1.Maximum = (int)dlina;

            double tekPosition = pl.controls.currentPosition;
            trackBar1.Value = (int)tekPosition;

            label7.Text = Convert.ToString((int)tekPosition / 60) + ":" + Convert.ToString((int)tekPosition % 60);
            label8.Text = Convert.ToString((int)dlina / 60) + ":" + Convert.ToString((int)dlina % 60);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (!pause)
            {
                pl.controls.currentPosition = trackBar1.Value;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
