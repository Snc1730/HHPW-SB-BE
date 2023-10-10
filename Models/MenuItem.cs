namespace HHPW_SB_BE.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
