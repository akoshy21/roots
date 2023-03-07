using System;
using UnityEngine;

public class RootTip : MonoBehaviour
{
 
    public void OnTriggerEnter2D(Collider2D other)
    {
      
    
    Pocket pocket = other.GetComponent<Pocket>();

        if (pocket)
        {
            pocket.DrainPocket();
            
        }
        
        Rock rock = other.gameObject.GetComponent<Rock>();

        if (rock)
        {
            this.GetComponentInParent<RootController>().DestroyRoot();
            rock.HitRock();
            AudioManager.instance.PlayOneShot(FMOD_Events.instance. RootHit, this.transform.position);

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