using System;
using System.Windows.Forms;
using System.Reflection;
using AxSHDocVw;

namespace WebBrowserOfficeIntegration
{
    public partial class Form1 : Form
    {
        private string tempFileName;
        private object oDocument;


        public Form1()
        {
            InitializeComponent();

            InitializedWebBrowser(tempFileName);

        }

        private void InitializedWebBrowser(string s)
        {
            //var uriCloud = new Uri(
            //    "https://coesia-my.sharepoint.com/:w:/r/personal/ali_abdulhussein_flexlink_com/_layouts/15/Doc.aspx?sourcedoc=%7BB8C4D61B-93AA-4DE2-8DDA-22E0449E9AA0%7D&file=WebBrowser.docx&action=default&mobileredirect=true");
            //var uriLocal = new Uri(String.Format("file:///{0}/test.html", "C:/Users/SEAAN0/Documents"));
            //var localFile = @"C:/Users/SEAAN0/Documents/TestWebBrowserObject.docx";
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            oDocument = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Browse";
            openFileDialog1.Filter = "Office Documents(*.doc,*.docx, *.xls, *.ppt)|*.doc;*.docx;*.xls;*.ppt";
            openFileDialog1.FilterIndex = 1;
        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            String strFileName;

            //Find the Office document.
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            strFileName = openFileDialog1.FileName;

            //If the user does not cancel, open the document.
            if (strFileName.Length != 0)
            {
                Object refmissing = System.Reflection.Missing.Value;
                oDocument = null;
                axWebBrowser1.Navigate(strFileName, ref refmissing, ref refmissing, ref refmissing, ref refmissing);
            }
        }


        private void axWebBrowser1_NavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e)
        {
            //Note: You can use the reference to the document object to 
            //      automate the document server.
            try
            {
                Object o = e.pDisp;

                oDocument = o.GetType().InvokeMember("Document", BindingFlags.GetProperty, null, o, null);

                //Object oApplication = o.GetType().InvokeMember("Application", BindingFlags.GetProperty, null, oDocument, null);

                //Object oName = o.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, oApplication, null);

                //MessageBox.Show("File opened by: " + oName.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}


