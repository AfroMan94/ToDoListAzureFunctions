using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using ToDoList.Functions.Commands;
using Newtonsoft.Json;
using ToDoList.Functions.Extensions;

namespace ToDoList.Functions.Functions
{
    public class MarkItemAsDoneFunction
    {
        private readonly IMediator _mediator;

        public MarkItemAsDoneFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("MarkItemAsDoneFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MarkItemAsCompletedCommand command = JsonConvert.DeserializeObject<MarkItemAsCompletedCommand>(requestBody);

            var result = await _mediator.Send(command);
            
            if (result.IsFailed)
            {
                return result.GetErrorResponse();
            }

            return new OkObjectResult("Item successfully marked as completed.");
        }
    }
}
