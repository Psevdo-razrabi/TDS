using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.Weapons.InterfaseWeapon;
using UI.Storage;
using UniRx;
using UnityEngine;

namespace Game.Player.Weapons.ReloadStrategy
{
    public class ReloadImage : IReloadStrategy
    {
        public readonly Subject<Unit> ReloadCompletedSubject = new();
        private const float duration = 5f; //в конфиг
        
        public async void Reload(ReloadComponent reloadComponent, BoolStorage boolStorage)  //реализация перезарядки с помощю картинки
        {
            boolStorage.ChangeBoolValue(true);
            reloadComponent.WeaponData.IsReloading = true;
            await ReloadWithImage(reloadComponent);
            Debug.LogWarning("Relooooooooooooooooooooad");
            ReloadCompletedSubject.OnNext(Unit.Default);
            boolStorage.ChangeBoolValue(false);
        }
        
        private async UniTask ReloadWithImage(ReloadComponent reloadComponent)
        {
            await DOTween
                .To(() => 0f, x => reloadComponent.ImageReloadValue.ChangeValue(x), 1f, duration)
                .SetEase(Ease.Linear);
            
            reloadComponent.WeaponData.IsReloading = false;
        }
    }
}