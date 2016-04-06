using System;

namespace InMemoryLocking
{
    public class InMemoryLock : IDisposable
    {
        private readonly InternalInMemoryLock _internalInMemoryLock;
        private readonly IInMemoryLockingService _inMemoryLockingService;

        public InMemoryLock(Guid lockKey, IInMemoryLockingService inMemoryLockingService)
        {
            Argument.Argument.CheckIfNull(lockKey, "lockKey");
            Argument.Argument.CheckIfNull(inMemoryLockingService, "inMemoryLockingService");

            _inMemoryLockingService = inMemoryLockingService;
            _internalInMemoryLock = _inMemoryLockingService.Lock(lockKey);
        }

        public void Dispose()
        {
            _inMemoryLockingService.Unlock(_internalInMemoryLock);
        }

        public bool IsValid => _inMemoryLockingService.CheckLock(_internalInMemoryLock);
    }
}
