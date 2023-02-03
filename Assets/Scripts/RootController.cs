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

    private Coroutine _drawing;

    private Vector3 _screenPoint, _original;
    private Vector3 _offset;

    private float _timeCount = 0.0f;
    private float _clickTime;

    private bool _dragging;
    private bool _click = false;


    void Update()
    {
        if (_click && Time.time > (_clickTime + clickDelta))
        {
            _click = false;
        }
    }

    private void OnMouseDown()
    {
        // Start drawing the root 
        _original = transform.position;
        StartRoot();

        if (_click && Time.time <= (_clickTime + clickDelta))
        {
            // If double click, split the root...
            Vector3 newPosLeft = _original + (Constants.LEFT_DOWN * splitDist);
            Vector3 newPosRight = _original + (Constants.RIGHT_DOWN * splitDist);

            GameObject left = Instantiate(gameObject);

            left.transform.position = newPosLeft;
            transform.position = newPosRight;

            DrawRoot(_original, newPosLeft);
            DrawRoot(_original, newPosRight);

            _click = false;
            _original = newPosRight;
        }
        else
        {
            _click = true;
            _clickTime = Time.time;
        }
    }

    void OnMouseDrag()
    {
        // Object is being dragged.
        _timeCount += Time.deltaTime;
        if (_timeCount > 0.25f)
        {
            //Debug.Log("Dragging:" + Input.mousePosition);
            _timeCount = 0.0f;
            _dragging = true;
        }

        if (_dragging)
        {
            transform.position = _original;
        }
    }

    private void OnMouseUp()
    {
        FinishRoot();
    }

    void StartRoot()
    {
        if (_drawing != null)
        {
            StopCoroutine(_drawing);
        }

        _drawing = StartCoroutine(DrawRoot());
    }

    void FinishRoot()
    {
        StopCoroutine(_drawing);
    }

    IEnumerator DrawRoot()
    {
        GameObject go = Instantiate(rootPrefab, rootParent.transform);
        LineRenderer line = go.GetComponent<LineRenderer>();

        line.positionCount = 0;
        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 updated = Vector3.Normalize(position - _original) * (speed * Time.deltaTime);
            updated.z = 0;
            line.positionCount++;
            _original += updated;
            _original.z = 0;
            line.SetPosition(line.positionCount - 1, _original);
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