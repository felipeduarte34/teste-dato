using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TesteBackend.Api.Controllers.v1.Pokemons.Requests;
using Xunit;

namespace TesteBackend.Api.IntegrationTests
{
    public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PokemonControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/v1/Pokemon?pokemonId=1")]
        public async Task GetByIdEndpoint_Should_ReturnOk(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/v1/Pokemon/all?Page=1&PageSize=1")]
        public async Task GetAllEndpoint_Should_ReturnOk(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory, ClassData(typeof(AddPokemonTestData))]
        public async Task AddEndpoint_Should_ReturnOk(string name, int hp, int attack, int defense, int specialAttack, int specialDefense, int speed)
        {
            // Arrange
            var client = _factory.CreateClient();
            const string url = "/api/v1/Pokemon";
            var request = new AddPokemonRequest { Name = name, Hp = hp, Attack = attack, Defense = defense, SpecialAttack = specialAttack, SpecialDefense = specialDefense, Speed = speed};
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, data);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        public class AddPokemonTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "sample pokemon name", 10, 10, 10, 10, 10, 10 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
