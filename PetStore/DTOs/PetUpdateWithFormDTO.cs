using PetStore.Enums;

namespace PetStore.DTOs
{
    public class PetUpdateWithFormDTO
    {
        public string Name { get; set; }
        public PetStatus Status { get; set; }

    }
}
