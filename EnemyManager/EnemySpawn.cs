using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public float wait = 3;    
    public GameObject enemy; 
    public int numberEnemy = 5;   
    public float rate = 1;  
    [Header("0 = No set")]
    [Tooltip("0: no custom")]
    public int customHealth = 0;
    [Tooltip("0: no custom")]
    public float customSpeed = 0;
    [Tooltip("0: no custom")]
    public float customAttackDmg = 0;
}
