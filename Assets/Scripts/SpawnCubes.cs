using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    public GameObject cube;
    public int rows;
    public int cols;
    float time;
    // Start is called before the first frame update
    void Awake()
    {
        time = DateTime.Now.Millisecond;

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < cols; z++)
            {
                GameObject instance = Instantiate(cube) as GameObject;
                Vector3 pos = new Vector3(x, Mathf.PerlinNoise(x * 0.21f, z * 0.21f), z);
                instance.transform.position = pos;
                //Debug.Log("for" + DateTime.Now.Millisecond);
            }
        }
        Debug.Log("Calcul time " + (DateTime.Now.Millisecond - time));
    }


}
