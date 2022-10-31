using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetHUD : MonoBehaviour
{
    public TextMeshPro nameText;
    public TextMeshPro levelText;
    public GameObject healthSlide;
    public float maxHP;

    // Update is called once per frame
    public void Setup(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "lvl " + unit.level;
        maxHP = unit.maxHP;
        healthSlide.transform.localScale = new Vector3(unit.curHP/maxHP, 0.9f, 1f);
    }

    public void SetHP(float hp)
    {
        healthSlide.transform.localScale = new Vector3(hp/maxHP, 0.9f, 1f);
    }
}
