using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Escape to leave...

public class RemoveDeadControllers : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        RootController rc = other.gameObject.GetComponent<RootController>();

        if (rc)
        {
            rc.DestroyRoot();
        }
    }
}
