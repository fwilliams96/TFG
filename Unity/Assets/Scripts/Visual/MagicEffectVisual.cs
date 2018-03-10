using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : MonoBehaviour
{
    private OneCreatureManager manager;
    private WhereIsTheCardOrEntity w;

    void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        w = GetComponent<WhereIsTheCardOrEntity>();
    }

    public void ActivateEffect()
    {
        float x = gameObject.transform.eulerAngles.x;
        float y = gameObject.transform.eulerAngles.y + 180;
        float z = gameObject.transform.eulerAngles.z;
        gameObject.transform.DORotate(new Vector3(x, y, z), 1);
    }
}
