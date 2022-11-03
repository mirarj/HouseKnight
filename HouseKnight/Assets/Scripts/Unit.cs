using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int level;
    public int atkDamagelow;
    public int atkDamagehigh;
    public float maxHP;
    public float curHP;
    public bool isPlayer;
    public float swingSpeed;

    public bool TakeDamage(float dmg)
    {
        if(isPlayer)
            SystemManager.curHP -= dmg;
    
        curHP -= dmg;
        if (curHP <= 0)
            return true;
        else
            return false;
    }

    void Start()
    {
        if(isPlayer)
        {
            curHP = SystemManager.curHP;
            maxHP = SystemManager.maxHP;
        }
    }
}
