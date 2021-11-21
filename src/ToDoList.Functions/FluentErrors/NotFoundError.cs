using FluentResults;

namespace ToDoList.Functions.FluentErrors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message)
        {
        }
    }
}
