using System;

namespace InMemoryLocking
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
            Argument.Argument.CheckIfNull(inMemoryLockingService, "inMemoryLockingService");

            _inMemoryLockingService = inMemoryLockingService;
        }

        public InMemoryLock ObtainLock(Guid lockKey)
        {
            return new InMemoryLock(lockKey, _inMemoryLockingService);
        }
    }
}