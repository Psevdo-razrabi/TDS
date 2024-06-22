using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCapsuleOverWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered into a body " + other.name);
        if(other.TryGetComponent(out MeshRenderer mesh))
        {
            mesh.materials[1].SetFloat("_Alpha", 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit from a body" + other.name);
        if (other.TryGetComponent(out MeshRenderer mesh))
        {
            mesh.materials[1].SetFloat("_Alpha", 0f);
        }
    }
}
