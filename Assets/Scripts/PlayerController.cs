using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Create a 2D Project and add a Canvas and an Image as a child. Position the Image in the center
// of the Canvas. Resize the Image to approximately a quarter of the height and width. Create a
// Resources folder and add a sprite. Set the sprite to the Image component. Then add this script
// to the Image. Then press the Play button. The Image should be clickable and moved with the
// mouse or trackpad.
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Vector3 position;
    private Vector3 original;
    private float timeCount = 0.0f;

    public GameObject rootParent;
    public LineRenderer rootPrefab;

    [SerializeField]
    private LineRenderer currentRoot;
    private bool dragging;
    
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        Debug.Log("Test");
        offset = gameObject.transform.position -
                 Camera.main.ScreenToWorldPoint(
                     new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        
        
        original = transform.position;
        Debug.Log("OnBeginDrag: " + position);
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
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            DrawLine(original, curPosition);

            transform.position = curPosition;
        }
    }

    private void OnMouseUp()
    {
        dragging = false;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            
        DrawLine(original, curPosition);
        currentRoot = null;
    }

    private void DrawLine(Vector3 orig, Vector3 target)
    {
        if (!currentRoot)
        {
            currentRoot = Instantiate(rootPrefab, rootParent.transform);
            currentRoot.positionCount = 2;
            currentRoot.startWidth = 0.15f;
            currentRoot.endWidth = 0.15f;
            currentRoot.SetPosition(0, orig);
            currentRoot.SetPosition(1, target);
        }
        else
        {
            currentRoot.SetPosition(1, target);
        }
    }
}