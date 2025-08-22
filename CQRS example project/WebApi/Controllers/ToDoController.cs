using CQRS_example_project.Application.Commands;
using CQRS_example_project.Application.Handlers;
using CQRS_example_project.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_example_project.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly CreateToDoHandler _createHandler;
        private readonly GetToDosHandler _getHandler;

        public ToDoController(CreateToDoHandler createHandler, GetToDosHandler getHandler)
        {
            _createHandler = createHandler;
            _getHandler = getHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoCommand command)
        {
            await _createHandler.Handle(command);
            return Ok("Created");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _getHandler.Handle(new GetToDoQuery());
            return Ok(todos);
        }
    }
}
