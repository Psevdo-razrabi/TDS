﻿using Zenject;

namespace Game.Player.Weapons.WeaponClass
{
    public sealed class Pistol : WeaponComponent
    {
        [Inject]
        public void Construct(ReloadComponent reloadComponent, FireComponent fireComponent)
        {
            this.reloadComponent = reloadComponent;
            this.fireComponent = fireComponent;
        }
    }
}