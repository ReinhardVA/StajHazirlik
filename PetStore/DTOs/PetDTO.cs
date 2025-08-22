using PetStore.Enums;

namespace PetStore.DTOs
{
    public class PetDTO
    {
        public CategoryDTO Category { get; set; }
        public string PetName { get; set; } 
        public List<string> PhotoUrls { get; set; }
        public List<TagDTO> Tags{ get; set; }
        public PetStatus Status { get; set; }
    }
}
