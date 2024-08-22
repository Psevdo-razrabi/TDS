using Game.Player.Interfaces;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class PlayerView : MonoBehaviour, IInitialaize
    {
        [field: SerializeField] public GameObject ModelRotate { get; private set; }
        public DashTrailEffect DashTrailEffect { get; private set; }
        public ValueCountStorage<float> ValueModelHealth { get; private set; }
        
        
        [Inject]
        public void Construct(DashTrailEffect dashTrailEffect) => DashTrailEffect = dashTrailEffect;
        
        public void InitModel(ValueCountStorage<float> valueCountStorage) => ValueModelHealth = valueCountStorage;
    }
}