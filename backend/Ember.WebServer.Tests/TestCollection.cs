using Xunit;

namespace Ember.WebServer.Tests;

[CollectionDefinition("TestCollection")]
public class TestCollection : ICollectionFixture<TestFixture>
{
}