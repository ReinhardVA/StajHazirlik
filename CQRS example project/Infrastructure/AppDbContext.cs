using CQRS_example_project.Domain;
using Microsoft.EntityFrameworkCore;

namespace CQRS_example_project.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ToDo> Todos { get; set; }
    }
}
