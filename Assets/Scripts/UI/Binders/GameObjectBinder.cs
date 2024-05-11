using System;
using MVVM;
using UniRx;
using UnityEngine;

namespace UI.Binders
{
    public class GameObjectBinder : IBinder, IObserver<Sprite>
    {
        private IReadOnlyReactiveProperty<Sprite> _gameObject;
        private GameObject _gameObjectView;
        private IDisposable _disposable;

        public GameObjectBinder(GameObject gameObjectView, IReadOnlyReactiveProperty<GameObject> gameObject)
        {
            _gameObjectView = gameObjectView;
            //_gameObject = gameObject;
        }
        
        public void Bind()
        {
            OnNext(_gameObject.Value);
            _disposable = _gameObject.Subscribe(this);
        }

        public void Unbind()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public void OnNext(Sprite value)
        {
            //_gameObjectView = value;
        }
        
        public void OnCompleted()
        {
            //non used
        }

        public void OnError(Exception error)
        {
            //non used
        }
    }
}