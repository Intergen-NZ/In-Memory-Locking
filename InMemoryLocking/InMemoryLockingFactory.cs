using System;
using Intergen.Common.Argument;

namespace Intergen.InMemoryLocking
{
    /// <summary>
    /// Responsible for retrieving a new lock on a particular GUID.
    /// Singleton Locking Service is injected into this factory.
    /// This factory should be used to obtain locks.
    /// </summary>
    public class InMemoryLockingFactory : IInMemoryLockingFactory
    {
        private readonly IInMemoryLockingService _inMemoryLockingService;

        public InMemoryLockingFactory(IInMemoryLockingService inMemoryLockingService)
        {
            Argument.CheckIfNull(inMemoryLockingService, "inMemoryLockingService");

            _inMemoryLockingService = inMemoryLockingService;
        }

        public InMemoryLock ObtainLock(Guid lockKey)
        {
            return new InMemoryLock(CleanUpStringKey(lockKey.ToString()), _inMemoryLockingService);
        }

        public InMemoryLock ObtainLock(string lockKey)
        {
            return new InMemoryLock(CleanUpStringKey(lockKey), _inMemoryLockingService);
        }

        private static string CleanUpStringKey(string lockKey)
        {
            return lockKey.ToLower().Trim();
        }
    }
}