namespace TcpLibrary
{
    public class Person
    {
        public Person(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public override string ToString()
        {
            return $"{Firstname} {Lastname}";
        }
    }
}