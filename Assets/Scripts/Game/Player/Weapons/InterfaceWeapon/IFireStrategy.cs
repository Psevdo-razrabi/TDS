using System;
using System.Collections.Generic;
using Input;

namespace Game.Player.Weapons.InterfaceWeapon
{
    public abstract class FireStrategy : IDisposable
    {
        //protected List<IDisposable> Subscriptions;
        protected InputSystemWeapon InputSystemWeapon;
        protected MouseInputObserver MouseInputObserver;
        
        protected FireStrategy(InputSystemWeapon inputSystemWeapon, MouseInputObserver mouseInputObserver)
        {
            InputSystemWeapon = inputSystemWeapon;
            MouseInputObserver = mouseInputObserver;
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