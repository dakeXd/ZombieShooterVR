using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementVR : MonoBehaviour
{

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float minAngle;
    [SerializeField] private SimpleShoot shoot;
    public bool lookingDown = false;


    private void Update()
    {
        lookingDown = mainCamera.transform.rotation.eulerAngles.x > minAngle && mainCamera.transform.rotation.eulerAngles.x < 180;
        
    }
    
    private void FixedUpdate()
    {
        
        if (lookingDown)
        {
           shoot.Reload();
        }

        
    }
}
