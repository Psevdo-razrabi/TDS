using CharacterOrEnemyEffect;
using UnityEngine;

namespace DI
{
    public class EffectInstaller : BaseBindings
    {
        [SerializeField] private DashTrailEffect _dashTrailEffect;
        public override void InstallBindings()
        {
            BindDashEffect();
            BindVFXCreate();
        }

        private void BindDashEffect() => BindInstance(_dashTrailEffect);

        private void BindVFXCreate() => BindNewInstance<CreateVFXTrail>();
    }
}