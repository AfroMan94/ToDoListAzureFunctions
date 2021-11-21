using FluentResults;

namespace ToDoList.Functions.FluentErrors
{
    class RuntimeError: Error
    {
        public RuntimeError(string message) : base(message)
        {
        }
    }
}
