using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int level;
    public int atkDamagelow;
    public int atkDamagehigh;

    [Header("Player Variables")]
    public float maxHP;
    public float curHP;
    public float curEXP;
    public float maxEXP;

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

    public void GainEXP(float exp)
    {
        SystemManager.curEXP += exp;
        Debug.Log(SystemManager.curEXP);
        curEXP += exp;
        curEXP = Mathf.Clamp(curEXP, 0, maxEXP + 1);
        transform.GetComponent<SetHUD>().SetEXP(this, curEXP, 0.8f);


        if (curEXP >= maxEXP)
        {
            StartCoroutine(LevelUp());
        }
        
    }

    IEnumerator LevelUp()
    {
        yield return new WaitForSeconds(0.5f);
        SystemManager.curHP += 10f; curHP += 10f;
        SystemManager.maxHP += 10f; maxHP += 10f;
        SystemManager.level++; level++;
        SystemManager.curEXP = 0f; curEXP = 0f;

        yield return new WaitForSeconds(0.4f);
        transform.GetComponent<SetHUD>().SetEXP(this, 0, 0.2f);
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
            curEXP = SystemManager.curEXP;
            maxEXP = SystemManager.maxEXP;
            level = SystemManager.level;
        }
    }

    void Update()
    {
        if(curHP <= maxHP*0.2f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
    }
}
