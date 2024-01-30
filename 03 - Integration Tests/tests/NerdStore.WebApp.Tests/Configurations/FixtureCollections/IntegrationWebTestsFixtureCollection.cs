using NerdStore.WebApp.MVC;

namespace NerdStore.WebApp.Tests.Configurations.FixtureCollections;

[CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }
