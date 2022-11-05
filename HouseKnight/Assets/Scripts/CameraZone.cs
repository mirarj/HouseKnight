using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public static bool zone;
    void OnTriggerEnter(Collider other)
    {
        zone = true;
        cam.Rotate(0f, -90f, 0f, Space.Self);
        player.Rotate(0f, -90f, 0f, Space.Self);
    }

    void OnTriggerExit(Collider other)
    {
        zone = false;
        cam.Rotate(0f, 90f, 0f, Space.Self);
        player.Rotate(0f, 90f, 0f, Space.Self);
    }

}
