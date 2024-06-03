using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponConfigs;

namespace Game.AsyncWorker.Interfaces
{
    public interface IAwaiter
    {
        UniTask AwaitLoadPlayerConfig(PlayerConfigs configs);
        UniTask AwaitLoadWeaponConfigs(WeaponConfigs configs);
        UniTask AwaitLoadShakeCameraConfigs(CameraShakeConfigs cameraShakeConfigs);
        UniTask AwaitLoadPrefabConfigs(WeaponPrefabs weaponPrefabs);
    }
}