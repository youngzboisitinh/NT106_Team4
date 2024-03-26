using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LMCB_TestForm
{
    public partial class Form1 : Form
    {
        private TcpClient tcpClient;
        private StreamReader sReader;
        private StreamWriter sWriter;
        private Thread clientThread;
        private int serverPort = 8000;
        private bool stopTcpClient = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void ClientRecv()
        {
            try
            {
                using (StreamReader sr = new StreamReader(tcpClient.GetStream()))
                {
                    while (!stopTcpClient)
                    {
                        string data = sr.ReadLine();
                        UpdateChatHistoryThreadSafe($"{data}\n");
                    }
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(ioEx.Message, "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }

        private delegate void SafeCallDelegate(string text);

        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.AppendText(text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string messageToSend = sendMsgTextBox.Text.Trim();

            // Kiểm tra xem tin nhắn có trống không
            if (string.IsNullOrEmpty(messageToSend))
            {
                MessageBox.Show("Please enter a message to send.");
                return;
            }

            // Tạo chuỗi tin nhắn có thêm thời gian hiện tại
            string formattedMessage = $"[{DateTime.Now:MM/dd/yyyy h:mm tt}] Client: {messageToSend}\n";

            // Hiển thị tin nhắn lên textBox của client
            sendMsgTextBox.Text = formattedMessage;

            try
            {
                sWriter.WriteLine(messageToSend);
                sWriter.Flush(); // Đảm bảo dữ liệu được gửi ngay lập tức
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Xóa nội dung trong textBox sau khi tin nhắn được gửi
            sendMsgTextBox.Clear();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopTcpClient = true;
            if (tcpClient != null)
                tcpClient.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                stopTcpClient = false;

                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(textBoxIP.Text), serverPort);
                sWriter = new StreamWriter(tcpClient.GetStream());
                sWriter.AutoFlush = true;
                sWriter.WriteLine(textBox1.Text);

                clientThread = new Thread(ClientRecv);
                clientThread.Start();
                MessageBox.Show("Connected");
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show(sockEx.Message, "Socket Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}