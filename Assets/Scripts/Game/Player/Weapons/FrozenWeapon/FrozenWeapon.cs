using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Player.Weapons.FrozenWeapon
{
    public class FrozenWeapon : BaseWeapon, IActivatable
    {
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private Transform _container;
        [SerializeField] private List<Transform>  _shootPoints = new List<Transform>();
        private WaitForSeconds _timeBetweenAttack;
        private Coroutine _attackRoutine;
        private Vector3 _direction;
        
        private void OnEnable()
        {
            Setup();
            Activate();
        }
        
        public void Activate() => _attackRoutine = StartCoroutine(Shoot());

        public void Deactivate() => StopCoroutine(_attackRoutine);

        private void Setup() => _timeBetweenAttack = new WaitForSeconds(WeaponStats[CurrentLevel-1].TimeBetweenAttack);

        public override void LevelUp()
        {
            base.LevelUp();
            Setup();
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                for (int i = 0; i < _shootPoints.Count; i++)
                {
                  
                    GameObject frozenBullet = _objectPool.GetFromPool();
                    frozenBullet.transform.SetParent(_container);
                    frozenBullet.transform.position = new Vector3(transform.position.x,1f,transform.position.z);
                    frozenBullet.transform.rotation = _shootPoints[i].localRotation;
                }
                yield return _timeBetweenAttack;
            }
        }
    }
}