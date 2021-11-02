using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    GameManager GameManager;

    public int X;
    public int Y;

    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        GameManager.OnCellClicked(X, Y);
    }
}
