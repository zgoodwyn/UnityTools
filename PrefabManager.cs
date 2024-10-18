using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance = null;

    public enum POOLITEM
    {
        Waypoint,
        SimpleUnit,
        SimpleTower,
        SimpleBullet,
        SimpleExplosion
    }

    public GameObject waypointFab;
    public GameObject SimpleUnitFab;
    public GameObject SimpleTowerFab;
    public GameObject SimpleBulletFab;
    public GameObject SimpleExplosionFab;

    public Dictionary<POOLITEM, GameObject> prefabDictionary;

    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(instance);
            }
        }
        instance = this;
        prefabDictionary = new Dictionary<POOLITEM, GameObject>() {
            { POOLITEM.Waypoint, instance.waypointFab},
            { POOLITEM.SimpleUnit, instance.SimpleUnitFab },
            { POOLITEM.SimpleTower, instance.SimpleTowerFab },
            { POOLITEM.SimpleBullet, instance.SimpleBulletFab },
            { POOLITEM.SimpleExplosion, instance.SimpleExplosionFab },
        };
    }

    private Dictionary<POOLITEM, RZTDPool> poolDictionary = new Dictionary<POOLITEM, RZTDPool>();

    public RZTDPool getPool(POOLITEM label, int itemCount)
    {
        RZTDPool pool = null;
        if(poolDictionary.TryGetValue(label, out pool))
        {
            pool.addItems(itemCount);
            return pool;
        }
        pool = new QueuePool(prefabDictionary[label], itemCount);
        poolDictionary.Add(label, pool);
        return pool;
    }

    public void clearPools()
    {
        foreach (RZTDPool pool in this.poolDictionary.Values)
        {
            pool.reclaimAll();
        }
    }
}
