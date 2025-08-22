using PetStore.Enums;

namespace PetStore.DTOs
{
    public class PetUpdateDTO
    {
        public int Id { get; set; }
        public CategoryDTO Category { get; set; }
        public string Name { get; set; }
        public List<string> PhotoUrls { get; set; }
        public List<TagDTO> Tags { get; set; }
        public PetStatus Status { get; set; }

    }
}
