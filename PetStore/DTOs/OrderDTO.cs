using System.ComponentModel.DataAnnotations;

namespace PetStore.DTOs
{
    public class OrderDTO
    {
        [Required]
        public int PetId { get; set; }
        [Required]
        [Range(1,100)]
        public int Quantity { get; set; }
        public DateTime? ShipDate { get; set; }
    }
}
