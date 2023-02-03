using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public float speed = 2f;
    public float clickDelta = 0.35f;

    public float splitDist = 1f;

    public GameObject rootPrefab;
    public GameObject rootParent;

    private Coroutine drawing;

    private Vector3 screenPoint, original;
    private Vector3 offset;

    private float timeCount = 0.0f;
    private float clickTime;

    private bool dragging;
    private bool click = false;

    private float zVal = 0;

    void Update()
    {
        if (click && Time.time > (clickTime + clickDelta))
        {
            click = false;
        }
    }

    private void OnMouseDown()
    {
        // Start drawing the root 
        original = transform.position;
        StartRoot();
        offset = original -
                 Camera.main.ScreenToWorldPoint(
                     new Vector3(Input.mousePosition.x, Input.mousePosition.y, zVal));
        offset.z = zVal;

        if (click && Time.time <= (clickTime + clickDelta))
        {
            // If double click, split the root...
            Vector3 newPosLeft = original + (Constants.LEFT_DOWN * splitDist);
            Vector3 newPosRight = original + (Constants.RIGHT_DOWN * splitDist);

            GameObject left = Instantiate(gameObject);

            left.transform.position = newPosLeft;
            transform.position = newPosRight;

            DrawRoot(original, newPosLeft);
            DrawRoot(original, newPosRight);

            click = false;
            original = newPosRight;
        }
        else
        {
            click = true;
            clickTime = Time.time;
        }
    }

    void OnMouseDrag()
    {
        // Object is being dragged.
        timeCount += Time.deltaTime;
        if (timeCount > 0.25f)
        {
            Debug.Log("Dragging:" + Input.mousePosition);
            timeCount = 0.0f;
            dragging = true;
        }

        if (dragging)
        {
            transform.position = original;
        }
    }

    private void OnMouseUp()
    {
        FinishRoot();
    }

    void StartRoot()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }

        drawing = StartCoroutine(DrawRoot());
    }

    void FinishRoot()
    {
        StopCoroutine(drawing);
    }

    IEnumerator DrawRoot()
    {
        GameObject go = Instantiate(rootPrefab, rootParent.transform);
        LineRenderer line = go.GetComponent<LineRenderer>();

        line.positionCount = 0;
        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 updated = Vector3.Normalize(position - original) * (speed * Time.deltaTime);
            position.z = 0;
            updated.z = 0;
            line.positionCount++;
            original += updated;
            line.SetPosition(line.positionCount - 1, original);
            yield return null;
        }
    }

    void DrawRoot(Vector3 pos1, Vector3 pos2)
    {
        GameObject go = Instantiate(rootPrefab, rootParent.transform);
        LineRenderer line = go.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);
    }
}