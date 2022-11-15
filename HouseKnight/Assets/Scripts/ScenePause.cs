using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePause : MonoBehaviour
{
    void PauseGame(){
      Time.timeScale = 0;
    }

    void PlayGame(){
      Time.timeScale = 1;
    }

    void Update(){
      //You can switch this input to whatever you want
      if (Input.GetKeyDown("q")){
        PauseGame();
        //You can switch the scene to whatever you want as well, we could
        //also make it accesable from unity and not through the code.
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
      }

      if (Input.GetKeyDown("e")){
        PlayGame();
      }
    }
}
