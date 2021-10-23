using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoading : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Vector3.back * Time.deltaTime * 25f);
    }
}
