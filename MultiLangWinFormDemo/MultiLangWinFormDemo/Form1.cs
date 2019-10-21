using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiLangWinFormDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializedForm(this);
            Form2.DoReloadEvent += Frm_DoReloadEvent;
        }

        private void Frm_DoReloadEvent()
        {
            ChangeLanguage("sv-SE");
        }

        private void btnEng_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en-US");
        }


        private void btnSe_Click(object sender, EventArgs e)
        {
            ChangeLanguage("sv-SE");
        }

        private void ChangeLanguage(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            //close child form????
            InitializedForm(this);
        }
        private void InitializedForm(Control form)
        {
            form.Controls.Clear();
            InitializeComponent();
            form.Refresh();
        }

        private void ShowNewForm_Click(object sender, EventArgs e)
        {
            var frm = new Form2();
            frm.Show();
        }
    }
}
