using System;
using FluentAssertions;
using NUnit.Framework;

namespace InMemoryLocking.UnitTests
{
    [TestFixture]
    public class InMemoryLockingFactoryUnitTests
    {
        private IInMemoryLockingFactory _lockingFactory;
        private IInMemoryLockingService _lockingService;

        [SetUp]
        public void SetUp()
        {
            _lockingService = new InMemoryLockingService(TimeSpan.FromSeconds(2));
            _lockingFactory = new InMemoryLockingFactory(_lockingService);
        }

        [Test]
        public void ObtainLock_Successful_ReturnsCustomLock()
        {
            var customLock = _lockingFactory.ObtainLock(Guid.NewGuid());
            customLock.IsValid.Should().BeTrue();
        }

        [Test]
        public void DisposeLock_Successful_UnlocksExistingLock()
        {
            var customLock = _lockingFactory.ObtainLock(Guid.NewGuid());
            using (customLock)
            {
                customLock.IsValid.Should().BeTrue();
            }
            customLock.IsValid.Should().BeFalse();
        }
    }
}
