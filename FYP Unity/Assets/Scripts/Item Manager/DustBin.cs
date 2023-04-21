using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBin : MonoBehaviour
{
    public void RemoveItem()
    {
        InventoryImageControl inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();
        inv.RemoveSelected(true);
    }
}