using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChaseTrigger : MonoBehaviour
{
    public int maxRandomRange;
    private int randomNum;
    private bool inLight = false;


    void FixedUpdate(){
      if (inLight){
        randomNum = Random.Range(1, maxRandomRange);
        if (randomNum == 1){
          Debug.Log("Working");
        }
      }
    }



    void OnTriggerEnter(Collider other){
      if (other.tag == "Player"){
        inLight = true;
      }
    }

    void OnTriggerExit(Collider other){
      if (other.tag == "Player"){
        inLight = false;
      }
    }
}
