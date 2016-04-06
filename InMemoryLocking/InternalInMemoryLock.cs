using System;

namespace InMemoryLocking
{
    public class InternalInMemoryLock
    {
        public Guid LockId { get; }
        public Guid LockKey { get; private set; }
        public DateTime LockedAt { get; private set; }

        public InternalInMemoryLock()
        {
            LockId = Guid.Empty;
        }

        public InternalInMemoryLock(Guid lockKey, Guid lockId, DateTime lockedAt)
        {
            LockedAt = lockedAt;
            LockKey = lockKey;
            LockId = lockId;
        }

        public static InternalInMemoryLock Empty => new InternalInMemoryLock();

        public bool IsEmpty => LockId == Guid.Empty;

    }
}