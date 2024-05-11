using System;
using MVVM;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Binders
{
    public class ButtonBinder : IBinder
    {
        private readonly Button _viewButton;
        private readonly UnityAction _modelAction;

        public ButtonBinder(Button viewButton, Action modelAction)
        {
            _viewButton = viewButton;
            _modelAction = new UnityAction(modelAction);
        }
        
        public void Bind()
        {
            _viewButton.onClick.AddListener(_modelAction);
        }

        public void Unbind()
        {
            _viewButton.onClick.RemoveListener(_modelAction);
        }
    }
}