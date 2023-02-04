using System;
using UnityEngine;

public class RootTip : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        // TODO: ADD ROCK COLLISION
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Pocket pocket = other.GetComponent<Pocket>();

        if (pocket)
        {
            pocket.DrainPocket();
        }
    }
    
    public void OnTriggerStay2D(Collider2D other)
    {
        Pocket pocket = other.GetComponent<Pocket>();

        if (pocket)
        {
            pocket.DrainPocket();
        }
    }
}