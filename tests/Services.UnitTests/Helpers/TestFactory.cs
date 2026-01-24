using Bogus;

namespace Services.UnitTests.Helpers
{
    public static class TestFactory
    {
        private static readonly Faker _fixture = new();

        public static T Create<T>() => _fixture.<T>();

        public static List<T> CreateList<T>(int count = 3) => _fixture.CreateMany<T>(count).ToList();
    }
}
