using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Apple,
    Cheese,
    Bottle
}

public class SelectableItem : MonoBehaviour
{
    
    public ItemType Type;

    // Start is called before the first frame update
    void Start()
    {
        Type = ItemType.Bottle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
