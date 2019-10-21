using System;

namespace MVPWinFormApp
{
    public class Person : IPerson
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }

        public Person() { }
        public Person(string firstName, string lastName, string emailAddress)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            FullName = GetFullName();
            EmailAddress = emailAddress;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
        public override string ToString()
        {
            return $"{Id}\n{FirstName}\n{LastName}\n{FullName}\n{EmailAddress}";
        }
    }
}
