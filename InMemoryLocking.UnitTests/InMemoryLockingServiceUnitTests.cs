using System;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace Intergen.InMemoryLocking.UnitTests
{
    [TestFixture]
    class InMemoryLockingServiceUnitTests
    {
        private IInMemoryLockingService _lockingService;

        [SetUp]
        public void SetUp()
        {
            _lockingService = new InMemoryLockingService(TimeSpan.FromSeconds(2));
        }

        [Test]
        public void Lock_Successful_ReturnsInternalLock()
        {
            var internalCustomLock = _lockingService.Lock(Guid.NewGuid().ToString());

            LockShouldBeValid(internalCustomLock);
        }

        [Test]
        public void Lock_LockExists_ThrowsCustomLockingException()
        {
            var key = Guid.NewGuid().ToString();
            _lockingService.Lock(key);
            _lockingService.Invoking(service => service.Lock(key))
                .ShouldThrow<InMemoryLockingException>()
                .WithMessage("*Could not obtain lock for key*");
        }

        [Test]
        public async Task Lock_TimedOutLockExists_ThrowsCustomLockingException()
        {
            var key = Guid.NewGuid().ToString();
            var timedOutLock = _lockingService.Lock(key);
            LockShouldBeValid(timedOutLock);

            await Task.Delay(TimeSpan.FromSeconds(3));
            var validLock = _lockingService.Lock(key);
            LockShouldBeValid(validLock);
            LockShouldBeInvalid(timedOutLock);
        }

        [Test]
        public void Unlock_Successful_ReleasesLock()
        {
            var internalCustomLock = _lockingService.Lock(Guid.NewGuid().ToString());
            LockShouldBeValid(internalCustomLock);

            _lockingService.Unlock(internalCustomLock);
            LockShouldBeInvalid(internalCustomLock);
        }

        [Test]
        public void Unlock_LockDoesntExist_ThrowsCustomLockingExceptions()
        {
            var internalCustomLock = new InternalInMemoryLock(Guid.NewGuid().ToString(), Guid.NewGuid(), DateTime.Now);

            _lockingService.Invoking(service => service.Unlock(internalCustomLock))
                .ShouldThrow<InMemoryLockingException>()
                .WithMessage("*Lock does not exist*");
        }

        [Test]
        public async Task Unlock_TimedOutLock_ThrowsCustomLockingException()
        {
            var lockKey = Guid.NewGuid().ToString();
            var timedOutLock = _lockingService.Lock(lockKey);
            LockShouldBeValid(timedOutLock);

            await Task.Delay(TimeSpan.FromSeconds(3));
            var validLock = _lockingService.Lock(lockKey);

            LockShouldBeValid(validLock);
            LockShouldBeInvalid(timedOutLock);

            _lockingService.Invoking(service => service.Unlock(timedOutLock))
                .ShouldThrow<InMemoryLockingException>()
                .WithMessage("*unauthorized lock removal*");
        }

        private void LockShouldBeValid(InternalInMemoryLock internalLock)
        {
            _lockingService.CheckLock(internalLock).Should().BeTrue();
        }

        private void LockShouldBeInvalid(InternalInMemoryLock internalLock)
        {
            _lockingService.CheckLock(internalLock).Should().BeFalse();
        }
    }
}