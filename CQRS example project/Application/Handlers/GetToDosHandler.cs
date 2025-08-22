using CQRS_example_project.Application.Queries;
using CQRS_example_project.Domain;
using CQRS_example_project.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CQRS_example_project.Application.Handlers
{
    public class GetToDosHandler
    {
        private readonly AppDbContext _appDbContext;

        public GetToDosHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ToDo>> Handle(GetToDoQuery query)
        {
            return await _appDbContext.Todos.ToListAsync();

        }

    }
}
