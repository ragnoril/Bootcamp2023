using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int TileType;

    public int GetX()
    {
        return (int)transform.localPosition.x;
    }

    public int GetY()
    {
        return (int)(transform.localPosition.y * -1);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
}
