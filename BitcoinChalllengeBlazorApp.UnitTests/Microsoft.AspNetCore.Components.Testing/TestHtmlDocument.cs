using HtmlAgilityPack;

namespace Microsoft.AspNetCore.Components.Testing
{
    internal class TestHtmlDocument : HtmlDocument
    {
        public TestHtmlDocument(TestRenderer renderer)
        {
            this.Renderer = renderer;
        }

        public TestRenderer Renderer { get; }
    }
}
