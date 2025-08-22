using CQRS_example_project.Application.Commands;
using CQRS_example_project.Domain;
using CQRS_example_project.Infrastructure;

namespace CQRS_example_project.Application.Handlers
{
    public class CreateToDoHandler
    {
        private readonly AppDbContext _appDbContext;

        public CreateToDoHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(CreateToDoCommand createToDoCommand)
        {
            var todo = new ToDo { Title = createToDoCommand.Title, IsCompleted = false };

            _appDbContext.Add<ToDo>(todo);
            //_appDbContext.Todos.Add(todo);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
