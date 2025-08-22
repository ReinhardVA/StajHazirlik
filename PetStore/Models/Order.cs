using PetStore.Enums;

namespace PetStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        public OrderStatus Status { get; set; }
        public bool Complete { get; set; }
    }
}
