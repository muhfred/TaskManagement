﻿namespace TaskManagement.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModifiedAt { get; set; }

    public string? LastModifiedBy { get; set; }
}

