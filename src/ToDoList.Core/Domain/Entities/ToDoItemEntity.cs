using System;
using Newtonsoft.Json;

namespace ToDoList.Core.Domain.Entities
{
    public sealed class ToDoItemEntity : BaseEntity
    {
        private ToDoItemEntity(string description)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            IsCompleted = false;
            CreatedOn = DateTime.Now;
        }

        //for serialization
        public ToDoItemEntity()
        {
        }

        public static ToDoItemEntity Create(string description)
        {
            return new ToDoItemEntity(description);
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedOn = DateTime.Now;
        }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; private set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; private set; }

        [JsonProperty("completedOn")]
        public DateTime CompletedOn { get; private set; }
    }
}