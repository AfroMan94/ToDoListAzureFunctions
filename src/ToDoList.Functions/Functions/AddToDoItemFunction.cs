using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ToDoList.Functions.Commands;
using ToDoList.Functions.Extensions;

namespace ToDoList.Functions.Functions
{
    public class AddToDoItemFunction
    {
        private readonly IMediator _mediator;

        public AddToDoItemFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("AddToDoItemFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AddToDoItemCommand command = JsonConvert.DeserializeObject<AddToDoItemCommand>(requestBody);

            var result = await _mediator.Send(command);
            if (result.IsFailed)
            {
                return result.GetErrorResponse();
            }

            return new OkObjectResult("Item successfully created.");
        }
    }
}