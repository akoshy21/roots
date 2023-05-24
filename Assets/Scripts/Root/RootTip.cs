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
            AudioManager.instance.PlayOneShot(FMOD_Events.instance.RootHit, this.transform.position);
            sr.color = onDeathColor;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {


        Pocket pocket = other.GetComponent<Pocket>();

        if (pocket)
        {
            pocket.DrainPocket();

            //Fmod Audio event call 
            //apple
          //  if (this.GetComponent<ItemSpawner>().GameObject.applePocket)
         //   {
          //      AudioManager.instance.PlayOneShot(FMOD_Events.instance.AppleHit, this.transform.position);
           // }

           // if (this.GetComponent<ItemSpawner>().GameObject.bonePocket)
          //  {
         //       AudioManager.instance.PlayOneShot(FMOD_Events.instance.BoneHit, this.transform.position);
          //  }

           // if (this.GetComponent<ItemSpawner>().GameObject.fishPocket)
          //  {
        //         AudioManager.instance.PlayOneShot(FMOD_Events.instance.LeafHit, this.transform.position);
        //    }

        }

    }
}