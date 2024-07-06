namespace DI
{
    public class PhysicsBinder : BaseBindings
    {
        public override void InstallBindings()
        {
            BindGravity();
        }

        private void BindGravity() => BindNewInstance<Gravity>();
    }
}