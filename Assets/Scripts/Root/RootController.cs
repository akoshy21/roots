using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using TMPro;

//TODO: Add a depth counter

public class RootController : MonoBehaviour
{
    public float speed = 2f;
    public float maxSpeed = 5f;
    public float clickDelta = 0.35f;

    public float splitDist = 1f;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

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

    public float maxThickness = 2f, minThickness = 0.5f;
    public float maxTime = 3;
    private float _timeOnRoot = 0;

    public List<LineRenderer> roots = new List<LineRenderer>();

    private Vector2 _direction;

    public bool active = true;
    
    //AudioFmod
    private EventInstance playerMovement;
    //

    private void Awake()
    {
        _cam = Camera.main.GetComponent<CameraController>();
        
    }

    
  
    private void Start()
    {
        PlantManager.Instance.AddRootController(this);
        playerMovement = AudioManager.instance.CreateInstance(FMOD_Events.instance.playerMovement);
    }

    void Update()
    {
        if (_click && Time.time > (_clickTime + clickDelta))
        {
            _click = false;
            //UpdateSound();  
            playerMovement = AudioManager.instance.CreateInstance(FMOD_Events.instance.playerMovement);
        }

    }
    

    private void FixedUpdate()
    {
        if (_direction.magnitude > 0 && PlantManager.GAME_ACTIVE)
        {
            rb.velocity = _direction * speed;
        }
    }
  
    private void OnBecameInvisible()
    {
        if(transform.position.y > Camera.main.transform.position.y)
            DestroyRoot();
        else
        {
            _cam.CheckY(transform.position);
        }
    }

    private void OnMouseDown()
    {
        rb.isKinematic = false;
        _timeOnRoot = 0;
            // Start drawing the root 
        _original = transform.position;
        StartRoot();

        if (_click && Time.time <= (_clickTime + clickDelta) )
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
            AudioManager.instance.PlayOneShot(FMOD_Events.instance.RootBreak, this.transform.position);
           
        }
        else
        {
            _click = true;
            _clickTime = Time.time;
        }
        PLAYBACK_STATE playbackState;
        playerMovement.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            playerMovement.start();
        }
    }

    void OnMouseDrag()
    {
        if (PlantManager.GAME_ACTIVE)
        {
            // Object is being dragged.
            _timeCount += Time.deltaTime;
            if (_timeCount > 0.25f)
            {
                //Debug.Log("Dragging:" + Input.mousePosition);
                _timeCount = 0.0f;
                _dragging = true;
                _timeOnRoot += Time.deltaTime;


                PLAYBACK_STATE playbackState;
                playerMovement.getPlaybackState(out playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    playerMovement.start();
                }
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
    }

    private void OnMouseUp()
    {
        playerMovement.stop(STOP_MODE.ALLOWFADEOUT);
        if (PlantManager.GAME_ACTIVE)
        {
            FinishRoot();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            _direction = Vector2.zero;
            if (active)
                _cam.CheckY(_original);



            rb.isKinematic = true;
        }
    }

    void OnMouseOver()
    {
        sr.color = new Color(152,255 , 150, 0.15f);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        sr.color = new Color(152,255 , 150, 0);
    }
    
    void StartRoot()
    {
        if (_drawing != null)
        {
            StopCoroutine(_drawing);
            ;
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

            float step = _timeOnRoot / maxTime;
            if (step > 1) step = 1;
            
            line.widthMultiplier = minThickness + maxThickness * step;
            
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
        sr.color = new Color(152,255 , 150, 0);

    }
    //fmodaudio
    private void UpdateSound()
    {

        if (speed!= 0 && _dragging)
        {

            PLAYBACK_STATE playbackState;
            playerMovement.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovement.start();
                playerMovement.release();
            }
        }
        else
        {
            playerMovement.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}