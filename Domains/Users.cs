namespace CQRSWithDocker_Identity.Domains
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Dept { get; set; } = default!;
        public decimal Salary { get; set; }

       
        public Users(string name, string dept, decimal salary)
        {
            Id = Guid.NewGuid();
            Name = name;
            Dept = dept;
            Salary = salary;
        }
    }
}
