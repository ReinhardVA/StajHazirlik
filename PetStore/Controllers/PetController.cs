using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.DTOs;
using PetStore.Enums;
using PetStore.Models;

namespace PetStore.Controllers
{

    [ApiController]
    [Route("pet")]
    public class PetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPet([FromBody] PetDTO petRequest)
        {
            var pet = new Pet
            {
                Category = new Category
                {
                    Id = petRequest.Category.Id,
                    CategoryName = petRequest.Category.Name,
                },
                PetName = petRequest.PetName,
                PhotoUrls = petRequest.PhotoUrls,
                Tags = petRequest.Tags.Select(t => new Tag
                {
                    Id = t.Id,
                    TagName = t.Name,
                }).ToList(),

                Status = petRequest.Status,

            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return Ok(pet);
        }
        [HttpPost("{petId}/uploadImage")]
        public async Task<IActionResult> UploadPetImage([FromRoute] int petId, [FromForm] IFormFile imageFile)
        {
            Pet pet = await GetPetById(petId);
            if (pet == null) return NotFound("Pet not found");

            if (imageFile == null || imageFile.Length == 0) return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            pet.PhotoUrls ??= new List<string>();
            pet.PhotoUrls.Add($"/uploads/{imageFile.FileName}");
            await _context.SaveChangesAsync();
            return Ok("Image uploaded successfully");
        }

        [HttpPost("{petId}")]
        public async Task<IActionResult> UpdatePetWithForm([FromRoute] int petId, [FromForm] PetUpdateWithFormDTO petUpdateRequest)
        {
            var pet = await GetPetById(petId);
            if (pet == null) return NotFound("Pet not found");
            pet.PetName = petUpdateRequest.Name;
            pet.Status = petUpdateRequest.Status;

            await _context.SaveChangesAsync();
            return Ok("Pet updated");

        }

        [HttpPut] 
        public async Task<IActionResult> UpdatePet([FromBody] PetUpdateDTO petUpdateRequest)
        {
            var pet = await GetPetById(petUpdateRequest.Id);
            
            if (pet == null) return NotFound("Pet not found");

            pet.Category = new Category { Id = petUpdateRequest.Category.Id, CategoryName = petUpdateRequest.Category.Name };
            pet.PetName = petUpdateRequest.Name;
            pet.PhotoUrls = petUpdateRequest.PhotoUrls;
            pet.Tags = petUpdateRequest.Tags.Select(t => new Tag
            {
                Id = t.Id,
                TagName = t.Name,
            }).ToList();
            pet.Status = petUpdateRequest.Status;

            await _context.SaveChangesAsync();
            return Ok("Pet updated");
        }

        [HttpGet("findByStatus")]
        public async Task<IActionResult> GetPetByStatus([FromQuery] List<PetStatus> status)
        {
            var pets = await _context.Pets.Where(p =>  status.Contains(p.Status)).ToListAsync();
            return Ok(pets);
        }
        
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPet([FromRoute] int petId)
        {
            Pet? pet = await GetPetById(petId);
            if (pet == null) return NotFound("Pet not found");
            return Ok($"Pet found: {pet.PetName}");
        }

        [HttpDelete("{petId}")]
        public async Task<IActionResult> RemovePet([FromRoute] int petId)
        {
            Pet? pet = await GetPetById(petId);
            if (pet == null) return NotFound("Pet not found");
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return Ok("Pet deleted successfully.");
        }

        private async Task<Pet> GetPetById(int petId)
        {
            Pet pet = await _context.Pets.FindAsync(petId);
            return pet == null ? throw new KeyNotFoundException("Pet not found") : pet;
        }

    }
}
