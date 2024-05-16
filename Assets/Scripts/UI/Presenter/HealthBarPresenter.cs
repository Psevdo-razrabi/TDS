using UI.Storage;
using UI.View;

namespace UI.ViewModel
{
    public class HealthBarPresenter
    {
        private readonly HealthBarView _imageView;
        private ValueCountStorage<float> _valueCountStorage;

        public HealthBarPresenter(ValueCountStorage<float> valueCountStorage, HealthBarView imageView)
        {
            _valueCountStorage = valueCountStorage;
            _imageView = imageView;
        }

        public void Initialize()
        {
            OnValueCountChange(_valueCountStorage.Value);
            _valueCountStorage.OnCountValueChange += OnValueCountChange;
        }

        public void Dispose()
        {
            _valueCountStorage.OnCountValueChange -= OnValueCountChange;
        }
        
        private void OnValueCountChange(float count)
        {
            _imageView.ImageFillAmount.Value = count;
        }
    }
}