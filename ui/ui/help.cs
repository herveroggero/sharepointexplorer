using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ui
{
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }

        private void help_Load(object sender, EventArgs e)
        {
            byte[] data = Properties.Resources.connect_help;
            MemoryStream stream = new MemoryStream(data);
            richTextBoxHelp.LoadFile(stream, RichTextBoxStreamType.RichText);
        }
    }
}
