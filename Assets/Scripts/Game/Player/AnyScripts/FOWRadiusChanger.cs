using Cysharp.Threading.Tasks;
using DG.Tweening;
using FOW;

namespace Game.Player
{
    public class FOWRadiusChanger
    {
        private readonly FogOfWarRevealer3D _revealer;

        public FOWRadiusChanger(FogOfWarRevealer3D revealer)
        {
            _revealer = revealer;
        }

        public async void ChangerRadius(float startValueRadius, float endValueRadius, float timeToMaxRadius)
        {
            await Change(startValueRadius, endValueRadius, timeToMaxRadius);
        }

        private async UniTask Change(float startValueRadius, float endValueRadius, float timeToMaxRadius)
        {
            await DOTween
                .To(() => startValueRadius, x => _revealer.ViewAngle = x,
                    endValueRadius, timeToMaxRadius);
        }
    }
}