using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFix : MonoBehaviour
{
    [SerializeField] Transform objTransform;

    void Update()
    {
        transform.rotation = objTransform.rotation;
    }
}
