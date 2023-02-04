using UnityEngine;

public class Rock: MonoBehaviour
{
    public void HitRock()
    {
        PlantManager.Instance.DeadRoot();
    }
}