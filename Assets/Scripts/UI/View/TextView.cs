using MVVM;
using TMPro;
using UnityEngine;

namespace UI.View
{
    public class TextView : MonoBehaviour
    {
        [Data("TextInScreen")] 
        public TextMeshProUGUI textView;
    }
}