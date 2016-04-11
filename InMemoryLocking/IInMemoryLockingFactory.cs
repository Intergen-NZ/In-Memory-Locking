using System;

namespace InMemoryLocking
{
    /// <summary>
    /// Responsible for retrieving a new lock on a particular GUID or unique string.
    /// Singleton Locking Service is injected into this factory.
    /// This factory should be used to obtain locks.
    /// </summary>
    public interface IInMemoryLockingFactory
    {
        /// <summary>
        /// Retrieves a lock on a particular id.
        /// </summary>
        /// <param name="lockKey">The key to lock on</param>
        /// <returns>An <see cref="InMemoryLock"/> for the particular guid</returns>
        InMemoryLock ObtainLock(Guid lockKey);

        /// <summary>
        /// Retrieves a lock on a particular id.
        /// </summary>
        /// <param name="lockKey">The key to lock on</param>
        /// <returns>An <see cref="InMemoryLock"/> for the particular guid</returns>
        InMemoryLock ObtainLock(string lockKey);
    }
}