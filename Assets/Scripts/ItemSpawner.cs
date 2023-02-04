using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs")] public GameObject waterPocket;
    public GameObject fishPocket, bonePocket, applePocket, rock;

    [Header("Depth Settings")] public float maxDepth;
    public float scaleModMin, scaleModMax;

    [Header("Water Spawn")] public float waterTolerance = 3;
    public float minWaterTotal, maxWaterTotal;
    public int minWaterCount, maxWaterCount;
    public float scaleMax = 1.5f;

    [Header("Nutrient")] public float nutriTolerance = 5;
    public float minNutriTotal, maxNutriTotal;
    public int minNutriCount, maxNutriCount;

    [Header("Rock")] public float rockTolerance = 2;
    public int minRockCount, maxRockCount;

    void MoveDown()
    {
        // Get the camera height
        float height = Screen.height;
        float width = Screen.width;

        // Now we get the position of the camera
        float camY = Camera.main.transform.position.y;
        float camX = Camera.main.transform.position.x;

        float rightBound = (camX + width / 2f);
        float leftBound = (camX - width / 2f);

        float newDepth = (camY + height / 2f) + 1;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, newDepth, 0));

        float pointOnCurve = PointOnCurve(worldPoint.y);
    }

    float PointOnCurve(float currentDepth)
    {
        float x = currentDepth / maxDepth;

        if (x > 5)
            return 4 * Mathf.Pow(x, 3);

        return 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}