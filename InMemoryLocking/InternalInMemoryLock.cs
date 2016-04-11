using System;

namespace Intergen.InMemoryLocking
{
    public class InternalInMemoryLock
    {
        public Guid LockId { get; }
        public string LockKey { get; private set; }
        public DateTime LockedAt { get; private set; }

        public InternalInMemoryLock()
        {
            LockId = Guid.Empty;
        }

        public InternalInMemoryLock(string lockKey, Guid lockId, DateTime lockedAt)
        {
            LockedAt = lockedAt;
            LockKey = lockKey;
            LockId = lockId;
        }

        public static InternalInMemoryLock Empty => new InternalInMemoryLock();

        public bool IsEmpty => LockId == Guid.Empty;

    }
}