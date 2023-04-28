using System;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities;

/// <summary>
/// Standard interface for an entity that MAY have an owner of type <typeparamref name="TOwner"/>.
/// </summary>
public interface IMayHaveOwner<TOwner> : IMayHaveOwner
{
    /// <summary>
    /// Reference to the owner.
    /// </summary>
    [CanBeNull]
    TOwner Owner { get; }
}

/// <summary>
/// Standard interface for an entity that MAY have an owner.
/// </summary>
public interface IMayHaveOwner
{
    Guid? OwnerId { get; }
}