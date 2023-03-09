using System;
using UnityEngine;

public class RootTip : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color onDeathColor;
 
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
            sr.color = onDeathColor;
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