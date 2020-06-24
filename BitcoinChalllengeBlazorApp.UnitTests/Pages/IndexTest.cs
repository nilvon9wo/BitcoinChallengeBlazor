using BitcoinChallenge.Entities;
using BitcoinChallengeBlazorApp;
using BitcoinChallengeBlazorApp.Services;
using Microsoft.AspNetCore.Components.Testing;
using Microsoft.JSInterop;
using Moq;
using Nancy.Json;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Index = BitcoinChallengeBlazorApp.Pages.Index;

namespace BitcoinChalllengeBlazorApp.UnitTests {
    public class IndexTest {
        readonly TestHost host = new TestHost();
        readonly decimal testAmount = 8448.947391885M;
        readonly int testRefreshRate = 10;

        [Fact]
        public void ItFetchesPriceAndSetsTheValue() {
            // Arrange
            _ = this.SetMockRuntime();
            _ = this.CreateMockHttpClientAsync();
            _ = this.CreateTimer();

            // Act
            RenderedComponent<Index> componentUnderTest = this.host.AddComponent<Index>();

            // Assert   
            string displayAmount = componentUnderTest.Find("p").InnerText.Split(new char[] { ' ' })[0];
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            CultureInfo provider = new CultureInfo("de-DE");

            decimal resultAmount = decimal.Parse(displayAmount, style, provider);
            decimal expectedAmount = decimal.Parse($"{this.testAmount:n2}");
            Assert.Equal(expectedAmount, resultAmount);

            Assert.Equal(2, MockBitcoinMessageHandler.requestCount);
        }

        public Mock<IJSRuntime> SetMockRuntime() {
            Mock<IJSRuntime> jsRuntimeMock = new Mock<IJSRuntime>();
            this.host.AddService(jsRuntimeMock.Object);
            return jsRuntimeMock;
        }

        private HttpClient CreateMockHttpClientAsync() {
            MockBitcoinMessageHandler mockHttpMessageHandler = new MockBitcoinMessageHandler(this.CreateMockResponse);
            HttpClient httpClient = new HttpClient(mockHttpMessageHandler);
            this.host.AddService(httpClient);
            return httpClient;
        }

        private Task<HttpResponseMessage> CreateMockResponse() {
            BitcoinPriceWrapper bitcoinPriceWrapper = new BitcoinPriceWrapper {
                Data = new BitcoinPrice {
                    Amount = this.testAmount.ToString()
                }
            };

            HttpResponseMessage mockResponse = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(new JavaScriptSerializer().Serialize(bitcoinPriceWrapper))
            };
            mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Task.FromResult(mockResponse);
        }

        private PeriodicTimer CreateTimer() {
            AppSettings appSettings = new AppSettings {
                RefreshTimeInSeconds = this.testRefreshRate
            };
            PeriodicTimer timer = new PeriodicTimer(appSettings);
            this.host.AddService(timer);
            return timer;
        }

    }
}
