using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefinedItem : MonoBehaviour
{
    [SerializeField] RefinedItemManager.RItems Ritemtype;
    [SerializeField] Sprite image;
    private int RitemID;

    private void Start()
    {
        RitemID = RefinedItemManager.instance.GetItemID(Ritemtype);
    }

    public void SetValues(RefinedItem ritem)
    {
        this.Ritemtype = ritem.Ritemtype;
        this.image = ritem.image;
        this.RitemID = ritem.RitemID;
    }

    public RefinedItemManager.RItems GetItemType()
    {
        return Ritemtype;
    }

    public int GetItemID()
    {
        return RitemID;
    }

    public Sprite GetImage()
    {
        return image;
    }
}
