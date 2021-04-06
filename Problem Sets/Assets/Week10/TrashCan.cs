using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (!Input.GetMouseButton(0))
        {
            ObjectPool.Despawn(other.gameObject);
        }
    }
}
