using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void Red()
    {
        material.color = Color.red;
    }
    public void Blue()
    {
        material.color = Color.blue;
    }

    public void Green()
    {
        material.color = Color.green;
    }

    public void DebugHit()
    {
        Debug.Log("Zombie apuntado");
    }
    
}
