using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetHUD : MonoBehaviour
{
    public TextMeshPro nameText;
    public TextMeshPro levelText;
    public TextMeshPro damageText;
    public GameObject healthSlide;
    public GameObject redSlide;
    public GameObject expSlide;
    public float maxHP;
    public float curEXP;

    public float tweenTime = 2f;

    // Update is called once per frame
    public void Setup(Unit unit, bool isPlayer)
    {
        nameText.text = unit.unitName;
        levelText.text = "lvl " + unit.level;
        maxHP = unit.maxHP;
        curEXP = unit.curEXP;
        if(isPlayer)
            expSlide.transform.localScale = new Vector3(curEXP/unit.maxEXP, 0.9f, 1f);
        healthSlide.transform.localScale = new Vector3(unit.curHP/maxHP, 0.9f, 1f);
        redSlide.transform.localScale = new Vector3(unit.curHP/maxHP, 0.9f, 1f);
    }

    public void SetHP(float hp, float time, int dmg)
    {
        LeanTween.scale(healthSlide, new Vector3(hp/maxHP, 0.9f, 1f), time).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scale(redSlide, new Vector3(hp/maxHP, 0.9f, 1f), time).setEase(LeanTweenType.easeInOutExpo).setDelay(0.05f);
        
        StartCoroutine(Damage(dmg));
    }

    public void SetEXP(Unit unit, float exp, float time)
    {
        levelText.text = "lvl " + unit.level;
        LeanTween.scale(expSlide, new Vector3(exp/unit.maxEXP, 0.9f, 1f), time).setEase(LeanTweenType.easeInOutExpo);
    }

    IEnumerator Damage(int dmg)
    {
        damageText.text = "-" + dmg;
        damageText.gameObject.SetActive(true);
        damageText.transform.GetComponent<Animator>().Play("Damage");
        yield return new WaitForSeconds(0.8f);
        damageText.gameObject.SetActive(false);
    }

}
