using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class HealthBarView : MonoBehaviour
    {
        public ReactiveProperty<float> ImageFillAmount { get; private set; } = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        [SerializeField] private Image _image;

        private void OnEnable()
        {
                ImageFillAmount
                .Subscribe(x => _image.fillAmount = x)
                .AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}