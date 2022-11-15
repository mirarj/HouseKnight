using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
      //Object variables
      public GameObject obstaclePrefab;
      public GameObject ground;
      private int rangeEnd;
      private Vector3 spawnPoint;
	  public GameObject timeText;
      public GameObject gameOverText;


      //Timing variables
      public float timelower = 0.5f;
      public float timeUpper = 1.2f;
      private float timeToSpawn;
      private float spawnTimer = 0f;
	  
	  
	  public int gameTime = 20;
      private float gameTimer = 0f;


      void Start(){
            spawnPoint = new Vector3(20,5,0);

       }
	   
	  void FixedUpdate(){
            timeToSpawn = Random.Range(timelower, timeUpper);
			float multiplier = 0.5f + gameTime/5;
			timeToSpawn = timeToSpawn*multiplier;
            spawnTimer += 0.01f;
            if (spawnTimer >= timeToSpawn){
                GameObject newObject = Instantiate(obstaclePrefab, spawnPoint, Quaternion.identity, ground.transform);
                //only if not about to loop
                spawnTimer = 0f;
            }
			
			gameTimer += 0.01f;
        }
	  
}
