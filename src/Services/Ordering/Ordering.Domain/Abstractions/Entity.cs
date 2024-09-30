﻿namespace Ordering.Domain.Abstractions
{
    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastTimeModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
