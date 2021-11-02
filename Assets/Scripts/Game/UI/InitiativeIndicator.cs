using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeIndicator : MonoBehaviour
{
    public SpriteRenderer indicator;

    public void Start()
    {
        indicator.sortingOrder = 32767;
    }

    public void Show()
    {
        indicator.gameObject.SetActive(true);
    }

    public void Hide()
    {
        indicator.gameObject.SetActive(false);
    }
}
