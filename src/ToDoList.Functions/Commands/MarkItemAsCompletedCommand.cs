using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoList.Core.Interfaces;
using ToDoList.Functions.FluentErrors;

namespace ToDoList.Functions.Commands
{
    public class MarkItemAsCompletedCommand : IRequest<Result>
    {
        public MarkItemAsCompletedCommand(string id)
            => Id = id;
        public string Id { get; set; }
    }

    public class MarkItemAsCompletedCommandHandler : IRequestHandler<MarkItemAsCompletedCommand, Result>
    {
        private readonly IToDoItemRepository _repository;
        private readonly ILogger _logger;

        public MarkItemAsCompletedCommandHandler(IToDoItemRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(MarkItemAsCompletedCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                return Result.Fail(new ValidationError("You must specify id"));
            }

            var entity = await _repository.GetItemAsync(request.Id);

            if (entity is null)
            {
                return Result.Fail(new NotFoundError($"Item with {request.Id} not found"));
            }

            entity.MarkAsCompleted();

            try
            {
                await _repository.UpdateItemAsync(request.Id, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail(new RuntimeError("Something went wrong during adding new item"));
            }

            return Result.Ok();
        }
    }
}
