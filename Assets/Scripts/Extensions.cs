using UnityEngine;

public static class Extensions
{
    public static void SetConditionalActive(this GameObject go, bool active)
    {
        if (!go.activeSelf && active)
            go.SetActive(true);
        else if (go.activeSelf && !active)
            go.SetActive(false);
    }
}