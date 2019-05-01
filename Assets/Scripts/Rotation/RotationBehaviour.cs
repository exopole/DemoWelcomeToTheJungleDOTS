using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    public Vector3 rotationVecteur = new Vector3(0,0,1);
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationVecteur, Space.World);       
    }
}
