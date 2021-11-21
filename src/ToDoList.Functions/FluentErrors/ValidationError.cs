using FluentResults;

namespace ToDoList.Functions.FluentErrors
{
    public class ValidationError : Error
    {
        public ValidationError(string message) : base(message)
        {
        }
    }
}
