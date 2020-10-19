using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] EffectPrefabs;

    public GameObject CreateEffectByIndex(Vector3 position, int index, float duration, bool isFacingRight=true)
    {
        GameObject g = Instantiate(EffectPrefabs[index], position, Quaternion.identity);
        if (!isFacingRight)
        {
            g.transform.localScale = new Vector3(-1, 1, 1);
        }
        Destroy(g, duration);
        return g;
    }
  
}
