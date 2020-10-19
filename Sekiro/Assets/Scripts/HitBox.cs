using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject owner;
    public int ownerCamp;
    public float damage;
    public bool canBlock;
    public bool canParry;

    public float flySpeed;

    public int HitEffectIndex = -1;
    public bool DestoryOnHit;
    public void InitHitBox(GameObject _owner,float _damage,int _ownerCamp,bool _canBlock,bool _canParry)
    {
        owner = _owner;
        ownerCamp = _ownerCamp;
        damage = _damage;
        canBlock = _canBlock;
        canParry = _canParry;
    }

    private void Update()
    {
        transform.position += Vector3.right * transform.localScale.x * flySpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attributes attributes = collision.GetComponent<Attributes>();
        if (attributes && attributes.camp!=ownerCamp)
        {
            attributes.TakeDamage(damage, owner,canBlock, canParry);
            if (DestoryOnHit)
            {
                Destroy(gameObject);
            }
            if (HitEffectIndex > -1)
            {
                EffectManager.instance.CreateEffectByIndex(transform.GetChild(0).position, HitEffectIndex, 1f);
            }
        }
    }
}
