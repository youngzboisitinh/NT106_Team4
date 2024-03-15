using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class calculator : Form
    {
        private string currentValue = "";

        public calculator()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            // Lấy giá trị của button được click
            string buttonText = ((Button)sender).Text;

            // Thêm giá trị của button vào chuỗi hiện tại
            currentValue += buttonText;

            // Hiển thị giá trị trên TextBox
            textBox1.Text = currentValue;
        }
    }
}
