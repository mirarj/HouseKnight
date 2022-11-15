using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMove : MonoBehaviour
{
    public float speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 travel = new Vector3(-speed * Time.deltaTime,0,0);
        transform.position = transform.position + travel;
        if (transform.position.x <= -20)
        {
            transform.position = new Vector3(20,0,0);
        }
    }
}
