using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteDesktopServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            new Viewer(int.Parse(textBoxPort.Text)).Show();
            btnListen.Enabled = false;
        }
    }
}
