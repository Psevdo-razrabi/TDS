using CharacterOrEnemyEffect;

namespace DI
{
    public class EffectInstaller : BaseBindings
    {
        public override void InstallBindings()
        { 
            BindVFXCreate();
        }

        private void BindVFXCreate() => BindNewInstance<CreateVFXTrail>();
    }
}