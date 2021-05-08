using Xunit;

namespace WebHostProject.Tests.Helpers
{
    [CollectionDefinition(nameof(NotThreadSafeResourceCollection), DisableParallelization = true)]
    public class NotThreadSafeResourceCollection
    {
    }
}
