using NerdStore.WebApp.MVC;

namespace NerdStore.WebApp.Tests.Configurations.FixtureCollections;

[CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }
