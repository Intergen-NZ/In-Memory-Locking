using System;
using System.Collections.Concurrent;
using Intergen.Common.Argument;

namespace Intergen.InMemoryLocking
{
    /// <summary>
    /// Responsible for maintaining, allocating and removing locks. 
    /// </summary>
    public class InMemoryLockingService : IInMemoryLockingService
    {
        private readonly TimeSpan _lockTimeout;
        private readonly ConcurrentDictionary<string, InternalInMemoryLock> _inMemoryLocks;

        /// <summary>
        /// Sets up a new instance of the locking service. Typically this would be a singleton instance.
        /// </summary>
        /// <param name="lockTimeout">LockTimeout is used to remove a lock if it is held too long and some other process tries to access it. The timed-out lock is only flagged, and the lock removed only when another process tries to access. Set to TimeSpan.Zero to disable lock timeout (although this can give issues if unlock fails and lock is never released).</param>
        public InMemoryLockingService(TimeSpan lockTimeout)
        {
            Argument.CheckIfNull(lockTimeout, "lockTimeout");

            _lockTimeout = lockTimeout;
            _inMemoryLocks = new ConcurrentDictionary<string, InternalInMemoryLock>();
        }

        public bool CheckLock(InternalInMemoryLock internalInMemoryLock)
        {
            Argument.CheckIfNull(internalInMemoryLock, "internalInMemoryLock");

            return CheckValidInternalLock(internalInMemoryLock);
        }

        public InternalInMemoryLock Lock(string lockKey)
        {
            Argument.CheckIfNull(lockKey, "lockKey");

            CheckAndRemoveTimedOutLock(lockKey);
            return AddLock(lockKey);
        }

        public void Unlock(InternalInMemoryLock internalInMemoryLock)
        {
            Argument.CheckIfNull(internalInMemoryLock, "internalInMemoryLock");

            if (!CheckValidInternalLock(internalInMemoryLock))
            {
                throw new InMemoryLockingException($"Lock does not exist or unauthorized lock removal for key [{internalInMemoryLock.LockKey}].");
            }

            RemoveLock(internalInMemoryLock);
        }

        /// <summary>
        /// Clears all locks from the repository.
        /// </summary>
        public void ClearLocks()
        {
            _inMemoryLocks.Clear();
        }

        private void RemoveLock(InternalInMemoryLock internalInMemoryLock)
        {
            InternalInMemoryLock savedLock;
            if (!_inMemoryLocks.TryRemove(internalInMemoryLock.LockKey, out savedLock))
            {
                throw new InMemoryLockingException($"Could not remove lock for key [{internalInMemoryLock.LockKey}].");
            }
        }

        private bool CheckValidInternalLock(InternalInMemoryLock internalInMemoryLock)
        {
            InternalInMemoryLock savedLock;
            if (!_inMemoryLocks.TryGetValue(internalInMemoryLock.LockKey, out savedLock))
            {
                return false;
            }

            return savedLock.LockId == internalInMemoryLock.LockId;
        }

        private InternalInMemoryLock GetLock(string lockKey)
        {
            InternalInMemoryLock savedLock;
            if (!_inMemoryLocks.TryGetValue(lockKey, out savedLock))
            {
                throw new InMemoryLockingException($"Could not find lock with key [{lockKey}].");
            }

            return savedLock;
        }

        private InternalInMemoryLock AddLock(string lockKey)
        {
            var internalInMemoryLock = new InternalInMemoryLock(lockKey, Guid.NewGuid(), DateTime.UtcNow);

            if (!_inMemoryLocks.TryAdd(lockKey, internalInMemoryLock))
            {
                throw new InMemoryLockingException($"Could not obtain lock for key [{lockKey}].");
            }

            return internalInMemoryLock;
        }

        private bool ContainsAnyLock(string lockKey)
        {
            return _inMemoryLocks.ContainsKey(lockKey);
        }

        private void CheckAndRemoveTimedOutLock(string lockKey)
        {
            if (_lockTimeout == TimeSpan.Zero || !ContainsAnyLock(lockKey)) return;

            var savedLock = GetLock(lockKey);

            if (DateTime.UtcNow.Subtract(savedLock.LockedAt).CompareTo(_lockTimeout) > 0)
            {
                RemoveLock(savedLock);
            }
        }
    }
}