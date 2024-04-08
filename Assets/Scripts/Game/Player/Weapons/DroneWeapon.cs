using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class DroneWeapon : BaseWeapon, IActivatable
    {
        [Header("Single")] 
        [SerializeField] private MeshRenderer _mesh1x;
        [SerializeField] private SphereCollider _collider1x;
        [SerializeField] private Transform _transform1x, _targetContainer1x;
        
        [Header("Double")] 
        [SerializeField] private List<MeshRenderer>  _mesh2x;
        [SerializeField] private List<SphereCollider>  _colliders2x;
        [SerializeField] private List<Transform>  _transform2x;
        [SerializeField] private Transform _targetContainer2x;

        private WaitForSeconds _interval, _duration, _timeBetweenAttack;
        private Coroutine _attackRoutine;
        
        private void Start()
        { 
            SetupWeapon();
            Activate();
        }


        private void Update() => transform.Rotate(0, WeaponStats[CurrentLevel - 1].Speed * Time.deltaTime, 0);

        public void Activate() => _attackRoutine = StartCoroutine(AttackCycle());

        public void Deactivate() => StopCoroutine(_attackRoutine);

        public override void LevelUp()
        {
            base.LevelUp();
            SetupWeapon();
        }

        private IEnumerator AttackCycle()
        {
            while (true)
            {
                if (CurrentLevel < 3)
                {
                    _mesh1x.enabled = !_mesh1x.enabled;
                    _collider1x.enabled =  _mesh1x.enabled;
                }
                else
                {
                    for (int i = 0; i < _mesh2x.Count; i++)
                    {
                        _mesh2x[i].enabled = !_mesh2x[i].enabled;
                        _colliders2x[i].enabled = _mesh2x[i].enabled;
                    }
                }
                yield return _interval = _mesh1x.enabled || _mesh2x[0].enabled ? _duration : _timeBetweenAttack;
            }
        }

        private void SetupWeapon()
        {
            _duration = new WaitForSeconds(WeaponStats[CurrentLevel - 1].Duration);
            _timeBetweenAttack = new WaitForSeconds(WeaponStats[CurrentLevel - 1].TimeBetweenAttack);
            if (CurrentLevel < 3)
            {
               _targetContainer1x.gameObject.SetActive(true);
               _targetContainer2x.gameObject.SetActive(false);
               _transform1x.localPosition = new Vector3(WeaponStats[CurrentLevel - 1].Range,1,0);
               _collider1x.center = new Vector3(WeaponStats[CurrentLevel - 1].Range,1,0);
            }
            else
            {
                _targetContainer1x.gameObject.SetActive(false);
                _targetContainer2x.gameObject.SetActive(true);
                for (int i = 0; i < _colliders2x.Count; i++) 
                    _colliders2x[i].gameObject.SetActive(true);
                
                _transform2x[0].localPosition = new Vector3(WeaponStats[CurrentLevel - 1].Range,0,0);
                _transform2x[1].localPosition = new Vector3(- WeaponStats[CurrentLevel - 1].Range,0,0);
                _colliders2x[0].center = new Vector3(WeaponStats[CurrentLevel - 1].Range,0,0);   
                _colliders2x[1].center = new Vector3(- WeaponStats[CurrentLevel - 1].Range,0,0);   
            }
        }
    }
}