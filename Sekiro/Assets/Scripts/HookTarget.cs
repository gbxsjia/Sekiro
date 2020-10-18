using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTarget : MonoBehaviour
{
    public static List<HookTarget> Hooks= new List<HookTarget>();
    [SerializeField]
    private SpriteRenderer IconRenderer;
    [SerializeField]
    private Sprite NormalIcon;
    [SerializeField]
    private Sprite DisableIcon;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float minDistance;
    private Vector3 direction;
    public bool Active;

    [SerializeField]
    private LayerMask traceLayer;

    private Character_Player player;
    private void Start()
    {
        player = PlayerInput.instance.GetComponent<Character_Player>();
        Hooks.Add(this);
    }
    private void Update()
    {
        direction = player.transform.position - transform.position;
        float distance = direction.magnitude;
        if(distance<= maxDistance && distance > minDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, traceLayer);
            if (!hit)
            {
                SetUse(true, true);
            }
            else
            {
                SetUse(true, false);
            }
        }
        else
        {
            SetUse(false, false);
        }
    }
    private void SetUse(bool visible,bool active)
    {
        Active = active;

        IconRenderer.enabled = visible;

        if (visible)
        {
            if (active)
            {
                IconRenderer.sprite = NormalIcon;
            }
            else
            {
                IconRenderer.sprite = DisableIcon;
            }
        }
    }
}
