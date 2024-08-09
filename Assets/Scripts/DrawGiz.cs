using System.Collections;
using System.Collections.Generic;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class DrawGiz : MonoBehaviour
{
    private WeaponData _weaponData;
    private Spread _spread;
    
    [Inject]
    private void Construct(WeaponData weaponData, Spread spread)
    {
        _weaponData = weaponData;
        _spread = spread;
    }
    private void OnDrawGizmos()
    {
        if (_weaponData != null)
        {
            if (_weaponData.BulletPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_weaponData.BulletPoint.position, 0.1f);
                
                Vector3 spread = _spread.CalculatingSpread();
                Vector3 startPosition = _weaponData.BulletPoint.position + spread;

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(startPosition, 0.1f);
            }
        }
    }
}
