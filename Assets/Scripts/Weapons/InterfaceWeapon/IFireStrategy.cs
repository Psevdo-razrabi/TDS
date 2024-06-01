using System;
using System.Collections.Generic;
using Input;

namespace Game.Player.Weapons.InterfaceWeapon
{
    public abstract class FireStrategy : IDisposable
    {
        protected FireComponent FireComponent;
        
        protected FireStrategy(FireComponent fireComponent)
        {
            FireComponent = fireComponent;
        }

        public abstract void Fire(FireComponent component);
        
        protected virtual void AddActionsCallbacks() {}
        
        protected virtual void RemoveActionCallbacks() {}

        public void Dispose()
        {
            RemoveActionCallbacks();
        }
    }
}