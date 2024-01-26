using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 1, 0);
    public float DestroyTime = 5f;

    void Start()
    {
        transform.localPosition += Offset;
        Destroy(gameObject, DestroyTime);
    }

}
