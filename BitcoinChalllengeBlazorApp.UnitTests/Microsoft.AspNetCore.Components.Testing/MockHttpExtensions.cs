﻿using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Microsoft.AspNetCore.Components.Testing
{
    public static class MockHttpExtensions
    {
        public static MockHttpMessageHandler AddMockHttp(this TestHost host)
        {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://example.com");
            host.AddService(httpClient);
            return mockHttp;
        }

        public static TaskCompletionSource<object> Capture(this MockHttpMessageHandler handler, string url)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            handler.When(url).Respond(() => tcs.Task.ContinueWith(task => {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(JsonSerializer.Serialize(task.Result))
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }));

            return tcs;
        }
    }
}
