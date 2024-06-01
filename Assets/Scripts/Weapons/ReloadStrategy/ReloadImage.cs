using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.Weapons.InterfaseWeapon;
using UI.Storage;
using UniRx;

namespace Game.Player.Weapons.ReloadStrategy
{
    public class ReloadImage : IReloadStrategy
    {
        private const float duration = 1f;
        private readonly Subject<Unit> _reloadCompletedSubject = new();
        public IObservable<Unit> ReloadCompleted => _reloadCompletedSubject;
        
        public async void Reload(ReloadComponent reloadComponent, BoolStorage boolStorage)
        {
            boolStorage.ChangeBoolValue(true);
            reloadComponent.WeaponData.IsReloading = true;
            await ReloadWithImage(reloadComponent);

            _reloadCompletedSubject.OnNext(Unit.Default);
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