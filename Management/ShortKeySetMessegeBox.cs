using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Management
{
    public partial class ShortKeySetMessegeBox : Form
    {
        string key = "";

        public ShortKeySetMessegeBox(string key)
        {
            InitializeComponent();

            this.key = key;
            if(!new FileInfo(Paths.shortKeyListPath + "\\" + key + ".txt").Exists)
            {
                new FileInfo(Paths.shortKeyListPath + "\\" + key + ".txt").Create();
            }
            else
            {
                TextBox.Text = File.ReadAllText(Paths.shortKeyListPath + "\\" + key + ".txt");
            }
        }

        private void SaveAndClose_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Paths.shortKeyListPath + "\\" + key + ".txt", TextBox.Text);

            Close();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShortKeySetMessegeBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
