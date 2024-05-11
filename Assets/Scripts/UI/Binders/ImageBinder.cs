using System;
using MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ImageBinder : IBinder, IObserver<float>
    {
        private IReadOnlyReactiveProperty<float> _image;
        private Image _imageView;
        private IDisposable _disposable;

        public ImageBinder(Image imageView, IReadOnlyReactiveProperty<float> image)
        {
            _image = image;
            _imageView = imageView;
        }

        public void Bind()
        {
            OnNext(_image.Value);
            _disposable = _image.Subscribe(this);
        }

        public void Unbind()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        public void OnNext(float value)
        {
            _imageView.fillAmount = value;
        }

        public void OnCompleted()
        {
            //nothing
        }

        public void OnError(Exception error)
        {
            //nothing
        }
    }
}