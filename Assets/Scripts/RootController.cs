using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public float speed = 2f;
    public float maxSpeed = 5f;
    public float clickDelta = 0.35f;

    public float splitDist = 1f;

    public Rigidbody2D rb;

    public GameObject rootControllerParent;
    public GameObject rootPrefab;
    public GameObject rootParent;

    private Coroutine _drawing;

    private Vector3 _screenPoint, _original;
    private Vector3 _offset;

    private float _timeCount = 0.0f;
    private float _clickTime;

    private bool _dragging;
    private bool _click = false;

    private CameraController _cam;

    public float maxThickness = 2f;
    public float maxTime = 3;
    private float _timeOnRoot = 0;

    public List<LineRenderer> roots = new List<LineRenderer>();

    private Vector2 _direction;

    public bool active = true;
    
    private void Awake()
    {
        _cam = Camera.main.GetComponent<CameraController>();
    }

    private void Start()
    {
        PlantManager.Instance.AddRootController(this);
    }

    void Update()
    {
        if (_click && Time.time > (_clickTime + clickDelta))
        {
            _click = false;
        }
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
                rb.AddForce(_direction * speed);
        }
    }

    private void OnMouseDown()
    {
        _timeOnRoot = 0;
            // Start drawing the root 
        _original = transform.position;
        StartRoot();

        if (_click && Time.time <= (_clickTime + clickDelta))
        {
            // If double click, split the root...
            Vector3 newPosLeft = _original + (Constants.LEFT_DOWN * splitDist);
            Vector3 newPosRight = _original + (Constants.RIGHT_DOWN * splitDist);

            GameObject left = Instantiate(gameObject, rootControllerParent.transform);

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
            _timeOnRoot += Time.deltaTime;
        }

        if (_dragging)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = Vector3.Normalize(position - _original);
            dir.y = Mathf.Clamp(dir.y, -1, -0.05f);
            _direction = dir;
          //  transform.position = _original;
        }
    }

    private void OnMouseUp()
    {
        FinishRoot();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        _direction = Vector2.zero;
        _cam.CheckY(_original);
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
        EdgeCollider2D edge = go.GetComponent<EdgeCollider2D>();

        line.positionCount = 0;
        roots.Add(line);

        while (true)
        {
            line.positionCount++;
            line.widthMultiplier += maxThickness * _timeOnRoot/maxTime;
            
            _original = transform.position;
            _original.z = 0;
            line.SetPosition(line.positionCount - 1, _original);
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);

            edge.points = ToV2(positions);
            
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

        roots.Add(line);
    }

    Vector2[] ToV2(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v2.Length; i++)
        {
            v2[i] = v3[i];
        }

        return v2;
    }

    public void DestroyRoot()
    {
        active = false;
        rb.simulated = false;
    }
}