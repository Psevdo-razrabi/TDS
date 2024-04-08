using System.Collections.Generic;
using Game.Player.Weapons;
using UnityEngine;

namespace Game.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private List<BaseWeapon> _weapons = new List<BaseWeapon>();
        private void Update() => Shoot();

        private void Shoot()
        {
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < _weapons.Count; i++)
                {
                 
                        _weapons[i].Shoot();
                }
            }
        }
    }
}