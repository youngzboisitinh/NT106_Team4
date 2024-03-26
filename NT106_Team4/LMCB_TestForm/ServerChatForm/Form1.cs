﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerChatForm
{
    public partial class Form1 : Form
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopChatServer = true;
        private readonly int _serverPort = 8000;
        private Dictionary<string,TcpClient> dict = new Dictionary<string,TcpClient>();
        public Form1()
        {
            InitializeComponent();
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(textBox1.Text), _serverPort));
                tcpListener.Start();

                while (!stopChatServer)
                {
                    //Application.DoEvents();
                    TcpClient _client = tcpListener.AcceptTcpClient();

                    StreamReader sr = new StreamReader(_client.GetStream());
                    StreamWriter sw = new StreamWriter(_client.GetStream());
                    sw.AutoFlush = true;
                    string username = sr.ReadLine();
                    if (username == null)
                    {
                        sw.WriteLine("Please pick a username");
                    }
                    else
                    {
                        if (!dict.ContainsKey(username))
                        {
                            Thread clientThread = new Thread(() => this.ClientRecv(username, _client));
                            dict.Add(username, _client);
                            clientThread.Start();
                        }
                        else
                        {
                            sw.WriteLine("Username already exist, pick another one");
                        }
                    }

                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClientRecv(string username, TcpClient tcpClient)
        {
            StreamReader sr = new StreamReader(tcpClient.GetStream());
            try
            {
                while (!stopChatServer)
                {
                    Application.DoEvents();
                    string msg = sr.ReadLine();
                    string formattedMsg = $"[{DateTime.Now:MM/dd/yyyy h:mm tt}] {username}: {msg}\n";
                    foreach (TcpClient otherClient in dict.Values)
                    {
                        StreamWriter sw = new StreamWriter(otherClient.GetStream());
                        sw.WriteLine(formattedMsg);
                        sw.AutoFlush = true;

                    }
                    
                    UpdateChatHistoryThreadSafe(formattedMsg);
                }
            }
            catch (SocketException sockEx)
            {
                tcpClient.Close();
                sr.Close();

            }

        }
        private delegate void SafeCallDelegate(string text);

        private void button1_Click(object sender, EventArgs e)
        {
            if (stopChatServer)
            {
                stopChatServer = false;
                listenThread = new Thread(this.Listen);
                listenThread.Start();
                MessageBox.Show(@"Start listening for incoming connections");
                button1.Text = @"Stop";
            }
            else
            {
                stopChatServer = true;
                button1.Text = @"Start listening";
                tcpListener.Stop();
                listenThread = null;
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có khách hàng nào kết nối hay không
            if (dict.Count == 0)
            {
                MessageBox.Show("No clients connected.");
                return;
            }

            // Nhập tin nhắn từ TextBox
            string messageToSend = textBox2.Text.Trim();

            // Kiểm tra xem tin nhắn có trống không
            if (string.IsNullOrEmpty(messageToSend))
            {
                MessageBox.Show("Please enter a message to send.");
                return;
            }

            // Tạo chuỗi tin nhắn có thêm thời gian hiện tại
            string formattedMessage = $"[{DateTime.Now:MM/dd/yyyy h:mm tt}] Server: {messageToSend}\n";

            // Gửi tin nhắn đến tất cả các khách hàng
            foreach (var client in dict.Values)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(client.GetStream());
                    sw.WriteLine(formattedMessage);
                    sw.Flush(); // Đảm bảo dữ liệu được gửi ngay lập tức
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu cần thiết
                    Console.WriteLine("Error sending message to client: " + ex.Message);
                }
            }

            UpdateChatHistoryThreadSafe(formattedMessage);
            textBox2.Clear();
        }
        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (textBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                TextBox3.Invoke(d, new object[] { text });
            }
            else
            {
                TextBox3.AppendText(text);
            }
        }
    }
}
