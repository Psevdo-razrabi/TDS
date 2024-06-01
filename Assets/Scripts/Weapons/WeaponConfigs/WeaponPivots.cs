using UnityEngine;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponPivots : MonoBehaviour
    {
        [field: SerializeField] public GameObject PistolPivot { get; private set; }
        [field: SerializeField] public GameObject RiflePivot { get; private set; }
        [field: SerializeField] public GameObject ShotgunPivot { get; private set; }
    }
}