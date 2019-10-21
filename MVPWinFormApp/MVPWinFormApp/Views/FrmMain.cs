using System;
using System.Windows.Forms;

namespace MVPWinFormApp
{
    public partial class FrmMain : Form, IFrmMainView
    {
        public string FirstName
        {
            get => txtFname.Text;
            set => txtFname.Text = value;
        }
        public string LastName
        {
            get => txtLname.Text;
            set => txtLname.Text = value;
        }
        public string FullName
        {
            get => txtFLname.Text;
            set => txtFLname.Text = value;
        }
        public string Email
        {
            get => txtEmail.Text;
            set => txtEmail.Text = value;
        }
        public FrmMain()
        {
            InitializeComponent();

        }

        public event EventHandler<EventArgs> SavePersonBtnClickEventRaised;
        protected virtual void OnSetPersonData()
        {
            SavePersonBtnClickEventRaised?.Invoke(this, EventArgs.Empty);
        }

        private IFrmMainPresenter _frmMainPresenter;
        public void FrmMain_Load(object sender, EventArgs e)
        {
            _frmMainPresenter = new FrmMainPresenter(this);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OnSetPersonData();
        }

    }
}
