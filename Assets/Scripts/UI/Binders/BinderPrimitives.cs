using System;
using MVVM;
using UniRx;

namespace UI.Binders
{
    public abstract class BinderPrimitives<T> : IBinder, IObserver<T>
    {
        protected IReadOnlyReactiveProperty<T> Primitives;
        protected T PrimitivesView;
        protected IDisposable Disposable;
        
        public abstract void Bind();

        public abstract void Unbind();

        public abstract void OnNext(T value);

        public virtual void OnCompleted()
        {
            //non used
        }

        public virtual void OnError(Exception error)
        {
            //non used
        }
    }
}