using UI.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace DI
{
    public class ViewInstaller : BaseBindings
    {
        [SerializeField] private GameObjectView gameObjectView;
        public override void InstallBindings()
        {
            BindView();
        }

        private void BindView() => BindInstance(gameObjectView);
        
    }
}