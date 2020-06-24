using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Components.Testing
{
    public class RenderedComponent<TComponent> where TComponent : IComponent
    {
        private readonly TestRenderer _renderer;
        private readonly ContainerComponent _containerTestRootComponent;
        private int _testComponentId;

        internal RenderedComponent(TestRenderer renderer)
        {
            this._renderer = renderer;
            this._containerTestRootComponent = new ContainerComponent(this._renderer);
        }

        public TComponent Instance { get; private set; }

        public string GetMarkup()
        {
            return Htmlizer.GetHtml(this._renderer, this._testComponentId);
        }

        internal void SetParametersAndRender(ParameterView parameters)
        {
            this._containerTestRootComponent.RenderComponentUnderTest(
                typeof(TComponent), parameters);
            (int, object) foundTestComponent = this._containerTestRootComponent.FindComponentUnderTest();
            this._testComponentId = foundTestComponent.Item1;
            this.Instance = (TComponent)foundTestComponent.Item2;
        }

        public HtmlNode Find(string selector)
        {
            return this.FindAll(selector).FirstOrDefault();
        }

        public ICollection<HtmlNode> FindAll(string selector)
        {
            // Rather than using HTML strings, it would be faster and more powerful
            // to implement Fizzler's APIs for walking directly over the rendered
            // frames, since Fizzler's core isn't tied to HTML (or HtmlAgilityPack).
            // The most awkward part of this will be handling Markup frames, since
            // they are HTML strings so would need to be parsed, or perhaps you can
            // pass through those calls into Fizzler.Systems.HtmlAgilityPack.

            string markup = this.GetMarkup();
            TestHtmlDocument html = new TestHtmlDocument(this._renderer);

            html.LoadHtml(markup);
            return html.DocumentNode.QuerySelectorAll(selector).ToList();
        }
    }
}
