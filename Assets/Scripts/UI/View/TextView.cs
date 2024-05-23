using MVVM;
using TMPro;
using UnityEngine;

namespace UI.View
{
    public class TextView : MonoBehaviour
    {
        [Data("DashInScreen")] 
        public TextMeshProUGUI dashView;
        [Data("AmmoInScreen")] 
        public TextMeshProUGUI ammoView;
    }
}