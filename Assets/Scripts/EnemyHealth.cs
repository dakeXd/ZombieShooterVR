using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject entity;
    private float actualHealth;

    private void Start()
    {
        actualHealth = maxHealth;
    }

    public bool getShooted(float dmg)
    {
        
        actualHealth -= dmg;
        Debug.Log("Enemigo disparado, vida actual " + actualHealth);
        if (actualHealth > 0) return false;
        Destroy(entity, 0.1f);
        return true;

    }
    
    
}
