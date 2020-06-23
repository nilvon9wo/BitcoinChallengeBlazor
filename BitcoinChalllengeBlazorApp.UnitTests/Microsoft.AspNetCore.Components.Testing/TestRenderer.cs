using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Testing
{
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    internal class TestRenderer : Renderer
    {
        private Exception _unhandledException;

        private TaskCompletionSource<object> _nextRenderTcs = new TaskCompletionSource<object>();

        public TestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId) {
            return base.GetCurrentRenderTreeFrames(componentId);
        }

        public int AttachTestRootComponent(ContainerComponent testRootComponent) {
            return this.AssignRootComponentId(testRootComponent);
        }

        public new Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
        {
            Task task = this.Dispatcher.InvokeAsync(
                () => base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs));
            this.AssertNoSynchronousErrors();
            return task;
        }

        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

        public Task NextRender => this._nextRenderTcs.Task;

        protected override void HandleException(Exception exception)
        {
            this._unhandledException = exception;
        }

        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
        {
            // TODO: Capture batches (and the state of component output) for individual inspection
            TaskCompletionSource<object> prevTcs = this._nextRenderTcs;
            this._nextRenderTcs = new TaskCompletionSource<object>();
            prevTcs.SetResult(null);
            return Task.CompletedTask;
        }

        public void DispatchAndAssertNoSynchronousErrors(Action callback)
        {
            this.Dispatcher.InvokeAsync(callback).Wait();
            this.AssertNoSynchronousErrors();
        }

        private void AssertNoSynchronousErrors()
        {
            if (this._unhandledException != null)
            {
                ExceptionDispatchInfo.Capture(this._unhandledException).Throw();
            }
        }
    }
}
