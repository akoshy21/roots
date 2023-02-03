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