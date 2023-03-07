using UnityEngine;

public class Rock: MonoBehaviour
{
    public void HitRock()
    {
        PlantManager.Instance.DeadRoot();
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.RockHit, this.transform.position);
    }
}