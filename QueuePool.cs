using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePool : SimplePool
{
    Queue<GameObject> availableItems = new Queue<GameObject>();
    HashSet<GameObject> itemSet = new HashSet<GameObject>();

    public QueuePool(GameObject o, int count) : base(o, count)
    {
        int index = 0;
        foreach (GameObject g in this.pool)
        {
            index++;
            g.AddComponent<RZTDPoolItem>().parentPool = this;
            availableItems.Enqueue(g);
            g.name = g.name + "_" + index;
            itemSet.Add(g);
        }

    }

    public override void addItems(int count)
    {

        for (int index = 0; index < count; index++)
        {
            myCount++;
            if (myCount > maxPoolSize)
            {
                myCount--;
                Debug.LogError("Max pool size reached using GameObject: " + this.prefab.name);
                return;
            }
            GameObject t = (GameObject)MonoBehaviour.Instantiate(this.prefab, Vector3.zero, Quaternion.identity);
            itemSet.Add(t);
            t.name = t.name + "_" + myCount;
            t.AddComponent<RZTDPoolItem>().parentPool = this;
            availableItems.Enqueue(t);
            t.transform.SetParent(poolLocation);
            t.SetActive(false);
            pool.Add(t);
        }
        myCount = myCount + count;
    }


    public override GameObject getNext(bool nonActive = false)
    {
        GameObject poolItem = null;
        if(availableItems.Count == 0)
        {
            addItems(1);
        }
        poolItem = availableItems.Dequeue();
        itemSet.Remove(poolItem);
      
        //Debug.Log(poolItem.name);
        return poolItem;
    }

    public override void reclaimItem(GameObject item)
    {
        item.transform.SetParent(poolLocation);
        item.SetActive(false);
        if (!itemSet.Contains(item))
        {
            availableItems.Enqueue(item);
            itemSet.Add(item);
        }
    }

    public override void reclaimAll()
    {
        foreach (GameObject o in this.pool)
        {
            this.reclaimItem(o);
        }
    }
}
