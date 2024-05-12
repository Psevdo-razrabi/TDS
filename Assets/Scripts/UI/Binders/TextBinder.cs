using System;
using MVVM;
using TMPro;
using UniRx;

namespace UI.Binders
{
    public class TextBinder : IBinder, IObserver<string>
    {
        private IReadOnlyReactiveProperty<string> _text;
        private TextMeshProUGUI _viewText;
        private IDisposable _disposable;
        
        public TextBinder(TextMeshProUGUI viewText, IReadOnlyReactiveProperty<string> text)
        {
            _viewText = viewText;
            _text = text;
        }

        public void Bind()
        {
            OnNext(_text.Value);
            _disposable = _text.Subscribe(this);
        }

        public void Unbind()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        public void OnNext(string value)
        {
            _viewText.text = value;
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