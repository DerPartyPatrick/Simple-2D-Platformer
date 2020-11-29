using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    public int rotationSpeed; 

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed); 
    }
}
