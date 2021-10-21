using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public int X;
    public int Y;

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
