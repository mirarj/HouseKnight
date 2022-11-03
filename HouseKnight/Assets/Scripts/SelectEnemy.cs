using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectEnemy : MonoBehaviour
{
    public Material[] mats;
    public SpriteRenderer sprite;
    //public GameObject lockon;
    public Cinemachine.CinemachineVirtualCamera c_VirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        sprite.material = mats[0];
    }

    void Update()
    {
        //lockon.transform.Rotate(0f, 0f, -0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        c_VirtualCamera.m_Follow = this.transform;
        //lockon.SetActive(true);
        sprite.material = mats[1];
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {  
            Destroy(this);
            SceneManager.LoadScene("Battle");
        }
    }

    void OnTriggerExit(Collider other)
    {
        //lockon.SetActive(false);
        sprite.material = mats[0];
        c_VirtualCamera.m_Follow = other.transform;
    }
}
