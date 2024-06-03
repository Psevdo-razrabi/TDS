using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class StorageView : MonoBehaviour
    {
        [Data("Pistol Equip")]
        public Button pistolEquip;
        [Data("Rifle Equip")]
        public Button rifleEquip;
        [Data("Shotgun Equip")]
        public Button shotgunEquip;
    }
}