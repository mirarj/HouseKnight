using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineShake : MonoBehaviour
{
    public static CineMachineShake Instance {get; private set;}
    CinemachineVirtualCamera cam;
    float shakeTimer;
    void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin camperlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        camperlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                //Timer Done
                CinemachineBasicMultiChannelPerlin camperlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camperlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
