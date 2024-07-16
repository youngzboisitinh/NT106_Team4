using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RemoteDesktopClient1
{
    public partial class Form1 : Form
    {
        private int portNumb;
        private readonly TcpClient client = new TcpClient();
        private NetworkStream stream;

        public Form1()


        {

            InitializeComponent();
        }
        private static Image GrabDesktop()
        {
            Rectangle bound = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bound.Width, bound.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(screenshot);
            g.CopyFromScreen(bound.X, bound.Y, 0, 0, bound.Size, CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        private void SendDesktopImage()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = client.GetStream();
            formatter.Serialize(stream, GrabDesktop());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            portNumb = int.Parse(textBoxPort.Text);
            try
            {
                client.Connect(textBoxIP.Text, portNumb);
                btnConnect.Text = "Connected";
                MessageBox.Show("Connected!");
                btnConnect.Enabled = false;
                btnShare.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Fail to Connect!");
                btnConnect.Text = "Not Connect";
            }
        }


        private void Client_Load(object sender, EventArgs e)
        {
            btnShare.Enabled = false;
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (btnShare.Text.StartsWith("Share"))
            {
                timer1.Start();
                btnShare.Text = "Stop Sharing";

            }
            else
            {
                timer1.Stop();
                btnShare.Text = "Share my screen";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendDesktopImage();
        }
    }
}
