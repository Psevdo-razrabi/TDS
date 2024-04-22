using System;
using Game.Player.Weapons.InterfaseWeapon;

namespace Game.Player.Weapons
{
    public class WeaponComponent : IWeapon, IFireMediator
    {
        private IFireStrategy _fireStrategy;
        
        public void Reload()
        {
            throw new System.NotImplementedException();
        }

        public void Fire()
        {
            _fireStrategy.Fire(this);
        }

        public void FireBullet()
        {
            //логика стрельбы конкретно спавн пули и дальнейшие действия
        }

        public void ChangeFireMode(IFireStrategy fireMediator)
        {
            _fireStrategy = fireMediator ?? throw new ArgumentNullException($"{(IFireStrategy)null} is null");
        }
    }
}