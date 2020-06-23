using BitcoinChallengeBlazorApp.Pages;
using Microsoft.AspNetCore.Components.Testing;
using System;
using Xunit;
using Index = BitcoinChallengeBlazorApp.Pages.Index;

namespace BitcoinChalllengeBlazorApp.UnitTests {
    public class UnitTest1 {
        TestHost host = new TestHost();

        [Fact]
        public void Test1() {
            RenderedComponent<Index> componentUnderTest = this.host.AddComponent<Index>();
            Assert.Equal("foo", componentUnderTest.Find("p").InnerText);
        }
    }
}
