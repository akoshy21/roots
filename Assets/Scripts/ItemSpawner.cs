using System;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public GameObject objectContainer;

    public RectTransform spawnWidth;

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

    public static ItemSpawner Instance;

    private Camera cam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        cam = Camera.main;
    }

    private void Start()
    {
        MoveDown();
    }

    public void MoveDown()
    {
        Physics2D.autoSyncTransforms = true;
        // Get the camera height
        float height = Screen.height;
        float width = spawnWidth.sizeDelta.x;

        // Now we get the position of the camera
        float camY = cam.transform.position.y;

        Vector2 min = spawnWidth.anchorMin;
        min.x *= Screen.width;
        min.y *= Screen.height;

        min += spawnWidth.offsetMin;

        Vector2 max = spawnWidth.anchorMax;
        max.x *= Screen.width;
        max.y *= Screen.height;

        max += spawnWidth.offsetMax;
        
        // TODO: make this the left and right bound of rect transform 
        Vector3 rightBound = cam.ScreenToWorldPoint(new Vector3(min.x, 0, 0));
        Vector3 leftBound = cam.ScreenToWorldPoint(new Vector3(max.x, 0, 0));
        // Debug.Log("BOUNDS " + rightBound.x + ", " + leftBound.x);

        Vector3 newDepth = cam.ScreenToWorldPoint(new Vector3(0, (camY - height) - 2, 0));

        float pointOnCurve = PointOnCurve(newDepth.y);

        int rockCount = (int) Mathf.Lerp(minRockCount, maxRockCount, pointOnCurve);
        rockCount = (int) Random.Range(rockCount - rockTolerance, rockCount + rockTolerance);

        if (rockCount < 0) rockCount = 0;

        for (int i = 0; i < rockCount; i++)
        {
            GameObject tempRock = Instantiate(rock, objectContainer.transform);
            tempRock.transform.position =
                GetAvailablePosition(tempRock.GetComponent<CircleCollider2D>(),
                    leftBound.x, rightBound.x, newDepth.y);
        }

        int nutriCount = (int) Mathf.Lerp(maxNutriCount, minNutriCount, pointOnCurve);
        nutriCount = (int) Random.Range(nutriCount - nutriTolerance, nutriCount + nutriTolerance);
        if (nutriCount < 0) nutriCount = 0;

        float nutriTotal = Mathf.Lerp(minNutriTotal, maxNutriTotal, pointOnCurve);
        nutriTotal = Random.Range(nutriTotal - (nutriTolerance * 5), nutriTotal + (nutriTolerance * 5));


        for (int i = 0; i < nutriCount; i++)
        {
            GameObject tempNutri = Instantiate(fishPocket, objectContainer.transform);
            tempNutri.transform.position =
                GetAvailablePosition(tempNutri.GetComponent<CircleCollider2D>(),
                    leftBound.x, rightBound.x, newDepth.y);
            tempNutri.GetComponent<Pocket>().SetTotal(nutriTotal);
        }

        int waterCount = (int) Mathf.Lerp(maxWaterCount, minWaterCount, pointOnCurve);
        waterCount = (int) Random.Range(waterCount - waterTolerance, waterCount + waterTolerance);
        if (waterCount < 0) waterCount = 0;

        float waterTotal = Mathf.Lerp(minWaterTotal, maxWaterTotal, pointOnCurve);
        waterTotal = Random.Range(waterTotal - (waterTolerance * 5), waterTotal + (waterTolerance * 5));


        for (int i = 0; i < waterCount; i++)
        {
            GameObject tempWater = Instantiate(waterPocket, objectContainer.transform);
            tempWater.transform.position =
                GetAvailablePosition(tempWater.GetComponent<CircleCollider2D>(),
                    leftBound.x, rightBound.x, newDepth.y);
            tempWater.GetComponent<Pocket>().SetTotal(waterTotal);
        }


        Physics2D.autoSyncTransforms = false;
    }

    Vector3 GetAvailablePosition(CircleCollider2D coll, float leftBound, float rightBound, float newDepth)
    {
        Vector3 rand = new Vector3(
            Random.Range(leftBound, rightBound), Random.Range(newDepth - 2f, newDepth + 2f), 0.1f);
        Collider2D hit = Physics2D.OverlapCircle(rand, coll.radius + 0.25f);
        if (hit)
        {
            return GetAvailablePosition(coll, leftBound, rightBound, newDepth);
        }

        return rand;
    }

    float PointOnCurve(float currentDepth)
    {
        float x = currentDepth / maxDepth;

        if (x < 0.5)
            return 4 * Mathf.Pow(x, 3);

        return 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}