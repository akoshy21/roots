using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float lowestY = 0;
    public float camSpeed = 1;

    private Vector3 endPos;
    private Vector3 origPos;

    private float step = 0;

    public void CheckY(Vector3 pos)
    {
        Debug.Log("check y " + lowestY  + pos.y);
        
        if (pos.y < lowestY)
        {
            lowestY = pos.y;
            origPos = transform.position;
            endPos = new Vector3(origPos.x, lowestY, -10);
            
            Debug.Log("lowestY " + lowestY + pos.y);

            step = 0;
        }
    }

    public void Update()
    {
        step += Time.deltaTime * camSpeed;
        if (step > 1) step = 1;
        if (Vector3.Distance(origPos, endPos) > 0.25f)
        {
            transform.position = Vector3.Lerp(origPos, endPos, step);
        }
    }
}