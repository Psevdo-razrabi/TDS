using MVVM;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace UI.View
{
    public class GameObjectView : MonoBehaviour
    {
        [Data("GameObject")]
        public GameObject GameObject;

        private BoolStorage _boolStorage;

        [Inject]
        public void Construct(BoolStorage boolStorage)
        {
            _boolStorage = boolStorage;
        }
        
        private void OnEnable()
        {
            _boolStorage.OnBoolValueChange += SetActiveGameObject;
            SetActiveGameObject(false);
        }

        private void OnDisable()
        {
            _boolStorage.OnBoolValueChange -= SetActiveGameObject;
        }

        private void SetActiveGameObject(bool obj)
        {
            GameObject.SetActive(obj);
        }
    }
}