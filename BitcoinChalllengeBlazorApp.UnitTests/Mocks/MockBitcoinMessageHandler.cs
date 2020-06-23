using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BitcoinChalllengeBlazorApp.UnitTests {
    public class MockBitcoinMessageHandler : HttpMessageHandler {
        public static int requestCount = 0;

        private readonly Func<Task<HttpResponseMessage>> createMockResponse;

        public MockBitcoinMessageHandler(Func<Task<HttpResponseMessage>> createMockResponse) {
            this.createMockResponse = createMockResponse;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken
            ) {
            requestCount++;
            return this.createMockResponse();
        }
    }
}