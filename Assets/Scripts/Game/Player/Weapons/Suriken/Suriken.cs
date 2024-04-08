using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.Suriken
{
    public class Suriken : Projectile
    {
        [SerializeField] private Transform _mesh;
        private SurikenWeapon _surikenWeapon;
        protected override void OnEnable()
        {
            Timer = new WaitForSeconds(_surikenWeapon.WeaponStats[_surikenWeapon.CurrentLevel - 1].Duration);
            Damage = _surikenWeapon.WeaponStats[_surikenWeapon.CurrentLevel - 1].Damage;
            base.OnEnable();
        }

        private void Update()
        {
            _mesh.Rotate(0,500f * Time.deltaTime,0);
            transform.position += transform.forward * (_surikenWeapon.WeaponStats[_surikenWeapon.CurrentLevel - 1].Speed * Time.deltaTime);
        }
        
        [Inject] private void Construct(SurikenWeapon surikenWeapon) => _surikenWeapon = surikenWeapon;
    }
}