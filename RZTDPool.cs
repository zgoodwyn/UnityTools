using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RZTDPool
{
    void addItems(int count);

    int getMaxItemCount();

    GameObject getNext(bool inactive);

    void reclaimItem(GameObject item);

    void reclaimAll();
}
