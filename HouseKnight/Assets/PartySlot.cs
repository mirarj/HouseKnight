using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{

    Slider health;
    Slider exp;
    Text hptext;
    Image bar;

    // Start is called before the first frame update
    void OnEnable()
    {
        health = transform.GetChild(1).GetComponent<Slider>();
        hptext = health.transform.GetChild(2).GetComponent<Text>();
        health.maxValue = SystemManager.maxHP;
        exp = transform.GetChild(2).GetComponent<Slider>();

        health.value = SystemManager.curHP;
        hptext.text = SystemManager.curHP + "/" + SystemManager.maxHP;

        bar = health.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        bar.color = Color.Lerp(Color.red, Color.green, health.value/health.maxValue);
    }
}
