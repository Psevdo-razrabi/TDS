using System;
using Customs;
using Cysharp.Threading.Tasks;
using Enemy.interfaces;
using UniRx;

namespace Enemy
{
    public class Die : IDie
    {
        public Subject<Unit> DieAction { get; private set; } = new ();
        private readonly RagdollHelper _ragdollHelper;

        public Die(RagdollHelper ragdollHelper)
        {
            _ragdollHelper = ragdollHelper;
        }

        public async UniTask Died()
        {
            DieAction.OnNext(Unit.Default);
            _ragdollHelper.SetActive();
            await UniTask.Delay(TimeSpan.FromMinutes(2f));
            _ragdollHelper.SetNotActive();
        }
    }
}