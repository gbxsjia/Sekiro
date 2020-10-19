using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public Character_Player player;
    private Vector3 Offset;
    [SerializeField]
    private CameraShake cameraShaker;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = PlayerInput.instance.GetComponent<Character_Player>();
        Offset = player.transform.position - transform.position;
    }
    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position - Offset;
        newPos.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, newPos, 0.15f);
    }
    public void CameraShake(float degree)
    {
        cameraShaker.UseCameraShake(degree);
    }
}
