namespace HHPW_SB_BE.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Uid { get; set; } 
        public string Name { get; set; }
        public bool isEmployee { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
