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
        private void operator_Click(object sender, EventArgs e)
        {
            // Lấy giá trị của button toán tử được click
            string operatorText = ((Button)sender).Text;
            if (operatorText == "−") // Kiểm tra nếu là dấu trừ
            {
                currentValue += " -";
            }
            else
            {
                currentValue += " " + operatorText + " ";
            }

            // Hiển thị giá trị trên TextBox
            textBox1.Text = currentValue;

        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Sử dụng DataTable.Compute để tính toán biểu thức trong chuỗi
            DataTable table = new DataTable();
            try
            {
                var result = table.Compute(currentValue, "");
                // Hiển thị kết quả trên TextBox
                textBox1.Text = result.ToString();
                // Reset giá trị và chuỗi hiện tại
                currentValue = result.ToString();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                textBox1.Text = "Error";
                currentValue = "";
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            currentValue = "";
            textBox1.Clear();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentValue))
            {
                // Xóa phần tử cuối cùng từ chuỗi hiện tại
                currentValue = currentValue.Substring(0, currentValue.Length - 1);
                // Hiển thị giá trị trên TextBox
                textBox1.Text = currentValue;
            }
        }

        private void calculator_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}       
