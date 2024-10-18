using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePool : RZTDPool
{
    protected static int maxPoolSize = 500;
    protected List<GameObject> pool;
    protected GameObject prefab;
    int currentIndex;
    protected int myCount;

    int tryStack = 0;
    protected Transform poolLocation;

    public SimplePool(GameObject o, int count)
    {
        poolLocation = new GameObject(o.name + " Pool").transform;
        poolLocation.SetParent(GameManager.instance.pooledItems);
        this.prefab = o;
        pool = new List<GameObject>();
        for (int index = 0; index < count; index++)
        {
            GameObject t = (GameObject)MonoBehaviour.Instantiate(o, Vector3.zero, Quaternion.identity);
            t.transform.SetParent(poolLocation);
            t.SetActive(false);
            pool.Add(t);
        }
        currentIndex = 0;
        myCount = count;
    }

    public virtual void addItems(int count)
    {
        for (int index = 0; index < count; index++)
        {
            myCount++;
            if(myCount > maxPoolSize)
            {
                myCount--;
                Debug.LogError("Max pool size reached using GameObject: " + this.prefab.name);
                return;
            }
            GameObject t = (GameObject)MonoBehaviour.Instantiate(this.prefab, Vector3.zero, Quaternion.identity);
            t.transform.SetParent(poolLocation);
            t.SetActive(false);
            pool.Add(t);
        }
        myCount = myCount + count;
    }

    public int getMaxItemCount()
    {
        return maxPoolSize;
    }

    public virtual GameObject getNext(bool nonActive = false)
    {
        GameObject t = pool[currentIndex];
        currentIndex++;
        if (currentIndex >= myCount)
        {
            currentIndex = 0;
        }

        if (nonActive)
        {
            if (!t.activeSelf)
            {
                tryStack = 0;
                return t;
            }
            else
            {
                if (tryStack >= myCount)
                {
                    Debug.Log("Critical Pool Failure!!!");
                    return null;
                }
                else
                {
                    tryStack++;
                    getNext(nonActive);
                }
            }
        }
        return t;
    }


    public virtual void reclaimAll()
    {
        for (int index = 0; index < myCount; index++)
        {
            pool[index].transform.SetParent(poolLocation);
            pool[index].SetActive(false);
        }
        currentIndex = 0;
    }

    public virtual void reclaimItem(GameObject item)
    {
        throw new System.NotImplementedException();
    }
}
