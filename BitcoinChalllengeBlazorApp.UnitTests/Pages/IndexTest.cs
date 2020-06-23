using BitcoinChallenge.Entities;
using BitcoinChallengeBlazorApp;
using Microsoft.AspNetCore.Components.Testing;
using Microsoft.JSInterop;
using Moq;
using Nancy.Json;
using RichardSzalay.MockHttp;
using System;
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
        public void Test1() {
            // Arrange
            _ = this.SetMockRuntime();
            _ = this.CreateMockHttpClientAsync();
            _ = this.CreateSettings();


            // Act
            RenderedComponent<Index> componentUnderTest = this.host.AddComponent<Index>();

            // Assert            
            Assert.Equal($"{this.testAmount:n2}", componentUnderTest.Find("p").InnerText);
        }

        public Mock<IJSRuntime> SetMockRuntime() {
            Mock<IJSRuntime> jsRuntimeMock = new Mock<IJSRuntime>();
            this.host.AddService(jsRuntimeMock.Object);
            return jsRuntimeMock;
        }

        public HttpClient CreateMockHttpClientAsync() {
            MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.When("https://api.coinbase.com/v2/prices/spot?currency=EUR")
                .Respond(this.CreateMockResponse);

            HttpClient httpClient = new HttpClient(mockHttpMessageHandler);
            this.host.AddService(httpClient);
            return httpClient;
        }

        private Task<HttpResponseMessage> CreateMockResponse() {
            BitcoinPriceWrapper bitcoinPriceWrapper = new BitcoinPriceWrapper();
            bitcoinPriceWrapper.Data = new BitcoinPrice();
            bitcoinPriceWrapper.Data.Amount = this.testAmount.ToString();

            HttpResponseMessage mockResponse = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(new JavaScriptSerializer().Serialize(bitcoinPriceWrapper))
            };
            mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Task.FromResult(mockResponse);
        }

        private BitcoinChallengeSettings CreateSettings() {
            AppSettings appSettings = new AppSettings {
                RefreshTimeInSeconds = this.testRefreshRate
            };
            BitcoinChallengeSettings bitcoinChallengeSettings = new BitcoinChallengeSettings(appSettings);
            this.host.AddService(bitcoinChallengeSettings);
            return bitcoinChallengeSettings;
        }

    }
}
