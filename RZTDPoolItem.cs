using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RZTDPoolItem : MonoBehaviour
{
    public RZTDPool parentPool;
    public List<TrailRenderer> trails;


    private void Awake()
    {
        trails = new List<TrailRenderer>(this.GetComponentsInChildren<TrailRenderer>(true));

    }

    public void OnDisable()
    {
        foreach (TrailRenderer tr in trails)
        {
            tr.Clear();
        }
        if (!this.gameObject.activeSelf)
        {
            parentPool.reclaimItem(this.gameObject);
        }
    }

    public void OnEnable()
    {
        foreach(TrailRenderer tr in trails)
        {
            tr.Clear();
        }
    }
}
