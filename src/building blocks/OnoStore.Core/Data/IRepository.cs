using OnoStore.Core.DomainObjects;
using System;

namespace OnoStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
    }
}
