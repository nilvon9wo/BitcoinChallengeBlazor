using BitcoinChallengeBlazorApp;
using Microsoft.AspNetCore.Components.Testing;
using Microsoft.JSInterop;
using Moq;
using System.Net.Http;
using Xunit;
using Index = BitcoinChallengeBlazorApp.Pages.Index;

namespace BitcoinChalllengeBlazorApp.UnitTests {
    public class IndexTest {
        TestHost host = new TestHost();

        [Fact]
        public void Test1() {
            Mock<IJSRuntime> jsRuntimeMock = new Mock<IJSRuntime>();
            this.host.AddService(jsRuntimeMock.Object);

            Mock<HttpClient> httpMock = new Mock<HttpClient>();
            this.host.AddService(httpMock.Object);

            int testRefreshRate = 10;
            AppSettings appSettings = new AppSettings();
            BitcoinChallengeSettings bitcoinChallengeSettings = new BitcoinChallengeSettings(appSettings);
            this.host.AddService(bitcoinChallengeSettings);

            RenderedComponent<Index> componentUnderTest = this.host.AddComponent<Index>();
            Assert.Equal("foo", componentUnderTest.Find("p").InnerText);
        }
    }
}
