using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoList.Core.Domain.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Functions.FluentErrors;

namespace ToDoList.Functions.Commands
{
    public class AddToDoItemCommand : IRequest<Result>
    {
        public AddToDoItemCommand(string description)
            => Description = description;
        public string Description { get; set; }
    }

    public class AddToDoItemCommandHandler : IRequestHandler<AddToDoItemCommand, Result>
    {
        private readonly ILogger _logger;
        private readonly IToDoItemRepository _repo;

        public AddToDoItemCommandHandler(ILogger logger, IToDoItemRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<Result> Handle(AddToDoItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Description is null)
            {
                return Result.Fail(new ValidationError("Description must be specified"));
            }

            var entity = ToDoItemEntity.Create(request.Description);
            try
            {
                await _repo.AddItemAsync(entity);
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
