using System;

namespace Intergen.InMemoryLocking
{
    public class InMemoryLockingException : Exception
    {
        public InMemoryLockingException()
        {
        }

        public InMemoryLockingException(string message) : base(message)
        {
        }
    }
}