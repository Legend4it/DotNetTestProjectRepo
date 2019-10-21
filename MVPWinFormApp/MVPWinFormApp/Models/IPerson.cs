using System;

namespace MVPWinFormApp
{
    public interface IPerson
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string FullName { get; set; }
        string EmailAddress { get; set; }
        string GetFullName();
        string ToString();
    }
}