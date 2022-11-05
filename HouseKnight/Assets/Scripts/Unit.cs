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
    public int critChance;

    public bool TakeDamage(float dmg)
    {
        if(isPlayer)
            SystemManager.curHP -= dmg;
    
        curHP -= dmg;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        if (curHP <= 0)
            return true;
        else
            return false;
    }

    public void Die()
    {
        LeanTween.rotate(gameObject, new Vector3(90f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.scale(gameObject, Vector3.zero, 1.5f).setEase(LeanTweenType.easeInOutQuint).setDelay(0.75f);
    }

    void Start()
    {
        if(isPlayer)
        {
            curHP = SystemManager.curHP;
            maxHP = SystemManager.maxHP;
        }
    }

    void Update()
    {
        if(curHP <= maxHP*0.2f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
    }
}
