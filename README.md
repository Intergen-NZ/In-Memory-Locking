# In Memory Locking

## NuGet
Add this library into your projects from [NuGet](https://www.nuget.org/packages/Intergen.InMemoryLocking/).

## Features
The purpose of this is to provide a light weight, thread-safe in-memory locking solution. If a lock on an id is already held and not timed-out, trying to get a lock on that same id from another thread will throw an exception. 

The timeouts are soft timeouts best explained with an example. If the global timeout is set to 5 seconds, and a lock is held for 10 seconds by an operation, then released, it will be successful.

If however in the lock in Request A has timed out (after five seconds) and another Request B tries to lock on that id, that will be given to Request B and the original lock for A will be invalidated. If the locks are used within transactions (like below), then any changes made in Request A will be rolled back. The timeout can be completely removed or set to whatever you need it to be.

## Example
It is recomended you initialize the service as a singleton at a global level.

```c#
var timeout = TimeSpan.FromSeconds(5);
var lockingService = new InMemoryLockingService(timeout);
```

Then create a factor and use as below:
```c#
var lockingFactory = new InMemoryLockingFactory(lockingService);
using(lockingFactory.ObtainLock(key))
{
	...	
}
```

The lock gets disposed automatically. You can clear all held locks by calling the ```lockingService.ClearAll()``` method.

## Transactions
Using this with transactions? The transaction will only complete/commit if a lock is held, the business logic executes cleanly, and the lock is released successfully. All other cases, the transaction will rollback.

```c#
using(var scope = new TransactionScope())
{
	using(lockingFactory.ObtainLock(key))
	{
		...
	}
	scope.Complete();
}
```

## Wish to contribute? 
Feel free to fork and make pull requests :)