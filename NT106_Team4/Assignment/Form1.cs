using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description="Chọn đường dẫn của bạn."}) 
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                { //Lấy đường dẫn của thư mục lên textbox
                    string selectedFolderPath = fbd.SelectedPath;

                    // Hiển thị đường dẫn của thư mục đã chọn trong TextBox
                    textBox1.Text = selectedFolderPath;
                    string[] files = Directory.GetFiles(selectedFolderPath);
                    if (files.Length == 0)
                    {
                        // Hiển thị thông báo nếu thư mục trống
                        MessageBox.Show("Thư mục trống.");
                    }
                    else
                    {
                        webBrowser1.Url = new Uri(fbd.SelectedPath);
                    }
                }
                    
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem WebBrowser có thể điều hướng trở lại không
            if (webBrowser1.CanGoBack)
            {
                // Nếu có thể, di chuyển đến trang trước đó
                webBrowser1.GoBack();
            }
            else
            {
                MessageBox.Show("Không thể quay lại trang trước đó.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            if (webBrowser1.CanGoForward)
            {
                
                webBrowser1.GoForward();
            }
            else
            {
                MessageBox.Show("Không thể đi tới trang tiếp theo.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
