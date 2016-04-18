using System;
using Intergen.Common;

namespace Intergen.InMemoryLocking
{
    public class InMemoryLock : IDisposable
    {
        private readonly InternalInMemoryLock _internalInMemoryLock;
        private readonly IInMemoryLockingService _inMemoryLockingService;

        public InMemoryLock(string lockKey, IInMemoryLockingService inMemoryLockingService)
        {
            Argument.CheckIfNull(lockKey, "lockKey");
            Argument.CheckIfNull(inMemoryLockingService, "inMemoryLockingService");

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
