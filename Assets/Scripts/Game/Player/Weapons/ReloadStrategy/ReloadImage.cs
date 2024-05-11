using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.Weapons.InterfaseWeapon;
using UI.Storage;
using UnityEngine;

namespace Game.Player.Weapons.ReloadStrategy
{
    public class ReloadImage : IReloadStrategy
    {
        private const float duration = 5f; //в конфиг
        
        public async void Reload(ReloadComponent reloadComponent, BoolStorage boolStorage)  //реализация перезарядки с помощю картинки
        {
            boolStorage.ChangeBoolValue(true);
            reloadComponent.WeaponData.IsReloading = true;
            await ReloadWithImage(reloadComponent);
            Debug.LogWarning("Relooooooooooooooooooooad");
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