namespace Intergen.InMemoryLocking
{
    public interface IInMemoryLockingService
    {
        /// <summary>
        /// Checks if the lock is valid by looking for the key then validating the lock id.
        /// </summary>
        /// <param name="internalInMemoryLock">The internal lock held inside <see cref="InMemoryLock"/>.</param>
        /// <returns>True if the lock is still valid, false if invalid.</returns>
        bool CheckLock(InternalInMemoryLock internalInMemoryLock);

        InternalInMemoryLock Lock(string lockKey);

        void Unlock(InternalInMemoryLock internalInMemoryLock);

        /// <summary>
        /// Removes all locks from the repository
        /// </summary>
        void ClearLocks();
    }
}