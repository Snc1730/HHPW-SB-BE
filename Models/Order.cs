namespace HHPW_SB_BE.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
        public string OrderName { get; set; }
        public DateTime DatePlaced { get; set; }
        public decimal Tip { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public string OrderType { get; set; }
        public string PaymentType { get; set; }
        public int? Review { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string OrderStatus { get; set; }
        public Dictionary<int, int> MenuItemQuantities { get; set; }
        public DateTime? DateClosed { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public Employee Employee { get; set; }
    }
}
