using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class collision : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private void Start()
    {

        _rigidbody.velocity = new Vector3(10f,0f,0f);
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(1);
    }
}
