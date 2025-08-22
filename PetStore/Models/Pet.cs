using PetStore.Enums;

namespace PetStore.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string PetName { get; set; }
        public List<string> PhotoUrls { get; set; }
        public List<Tag> Tags { get; set; }
        public PetStatus Status { get; set; }

    }
}
