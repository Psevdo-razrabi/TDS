using System;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.InterfaseWeapon;

namespace Game.Player.Weapons.Mediators
{
    public class MediatorFireStrategy
    {
        private IFireMediator _fireMediator;
        
        public MediatorFireStrategy(IFireMediator fireMediator)
        {
            _fireMediator = fireMediator ?? throw new ArgumentNullException($"{(IFireMediator)null} is null");
        }
        
        public void ChangeFireMode(FireStrategy strategy)
        {
            _fireMediator.ChangeFireMode(strategy);
        }
    }
}