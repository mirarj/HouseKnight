using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemy : MonoBehaviour
{
    public Material[] mats;
    public SpriteRenderer sprite;
    public GameObject lockon;

    // Start is called before the first frame update
    void Start()
    {
        sprite.material = mats[0];
    }

    void Update()
    {
        lockon.transform.Rotate(0f, 0f, -0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        lockon.SetActive(true);
        sprite.material = mats[1];
    }

    void OnTriggerExit(Collider other)
    {
        lockon.SetActive(false);
        sprite.material = mats[0];
    }
}
