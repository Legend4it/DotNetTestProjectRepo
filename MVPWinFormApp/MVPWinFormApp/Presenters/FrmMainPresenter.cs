using System;
using System.Net.Mail;

namespace MVPWinFormApp
{
    public class FrmMainPresenter : IFrmMainPresenter
    {
        public FrmMainPresenter(IFrmMainView view)
        {
            SubscribeToViewEvents(view);
        }

        public void SubscribeToViewEvents(IFrmMainView view)
        {
            view.SavePersonBtnClickEventRaised += new EventHandler<EventArgs>(OnSavePersonBtnClickEventRaised);
        }
        public void OnSavePersonBtnClickEventRaised(object sender, EventArgs e)
        {
            var objView = (IFrmMainView) sender;
            var person=new Person(objView.FirstName,objView.LastName,objView.Email);
            Console.WriteLine(person.ToString());

            UpdateView(person,objView);

        }

        private void UpdateView(IPerson person, IFrmMainView view)
        {
            view.FullName = person.FullName;
            if (!IsValidMail(person.EmailAddress))
            {
                view.Email = "Not Valid Email format";
            }
        }
        public bool IsValidMail(string email)
        {
            try
            {
                var mail=new MailAddress(email);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return false;
            }
        }
    }
}
