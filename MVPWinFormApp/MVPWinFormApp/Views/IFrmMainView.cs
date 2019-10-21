using System;

namespace MVPWinFormApp
{
    public interface IFrmMainView
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string FullName { get; set; }
        string Email { get; set; }

        event EventHandler<EventArgs> SavePersonBtnClickEventRaised;
    }
}
