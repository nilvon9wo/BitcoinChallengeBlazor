using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Components.Testing
{
    public class TestHost
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public TestHost()
        {
            this._serviceProvider = new Lazy<IServiceProvider>(() => this._serviceCollection.BuildServiceProvider());

            this._renderer = new Lazy<TestRenderer>(() =>
            {
                ILoggerFactory loggerFactory = this.Services.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(this.Services, loggerFactory);
            });
        }

        public IServiceProvider Services => this._serviceProvider.Value;

        public void AddService<T>(T implementation) {
            this.AddService<T, T>(implementation);
        }

        public void AddService<TContract, TImplementation>(TImplementation implementation) where TImplementation: TContract
        {
            if (this._renderer.IsValueCreated)
            {
                throw new InvalidOperationException("Cannot configure services after the host has started operation");
            }

            _ = this._serviceCollection.AddSingleton(typeof(TContract), implementation);
        }

        public void WaitForNextRender(Action trigger)
        {
            System.Threading.Tasks.Task task = this.Renderer.NextRender;
            trigger();
            _ = task.Wait(millisecondsTimeout: 1000);

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>() where TComponent: IComponent
        {
            RenderedComponent<TComponent> result = new RenderedComponent<TComponent>(this.Renderer);
            result.SetParametersAndRender(ParameterView.Empty);
            return result;
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>(ParameterView parameters) where TComponent : IComponent
        {
            RenderedComponent<TComponent> result = new RenderedComponent<TComponent>(this.Renderer);
            result.SetParametersAndRender(parameters);
            return result;
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>(IDictionary<string, object> parameters) where TComponent : IComponent
        {
            return this.AddComponent<TComponent>(ParameterView.FromDictionary(parameters));
        }

        private TestRenderer Renderer => this._renderer.Value;
    }
}
