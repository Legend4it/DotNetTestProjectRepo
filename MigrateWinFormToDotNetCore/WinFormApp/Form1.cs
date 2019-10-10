using System;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetText(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetText(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetText(sender);
        }

        private string GetText(object sender) => sender switch
        {
            Button button => textBox1.Text=button.Text+34343,
            _ =>string.Empty
        };
    }
}
