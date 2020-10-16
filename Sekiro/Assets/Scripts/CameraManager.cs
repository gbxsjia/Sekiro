using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField]
    private CameraShake cameraShaker;
    private void Awake()
    {
        instance = this;
    }

    public void CameraShake(float degree)
    {
        cameraShaker.UseCameraShake(degree);
    }
}
