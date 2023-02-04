using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomFillBar : MonoBehaviour
{
    public float yHeightMin;
    public float yHeightMax;
    private float yHeightCurrent = 0;
    private float yLastHeight = 0;
    private float yTargetHeight = 0;
    
    public float fillSpeed = 1;

    private RectTransform rect;

    [SerializeField]
    private float fillValue = 0;

    private float step = 0;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        yHeightCurrent = Mathf.Lerp(yHeightMin, yHeightMax, fillValue);
        yLastHeight = yHeightCurrent;
        transform.position = new Vector3(transform.position.x, yHeightCurrent, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        step += Time.deltaTime * fillSpeed;
        if (step > 1) step = 1;
        
        // TODO: CHANGE TO DIFF BETWEEN CURRENT HEIGHT AND ADJUSTED.
        if (Mathf.Abs(yTargetHeight - yHeightCurrent) > 0.01f)
        {
            yHeightCurrent = Mathf.Lerp(yLastHeight, yTargetHeight, step);
            rect.anchoredPosition = new Vector3(transform.localPosition.x, yHeightCurrent, transform.localPosition.z);
        }
    }

    public void SetFill(float value)
    {
        fillValue = value;
        yLastHeight = yHeightCurrent;
        yTargetHeight = Mathf.Lerp(yHeightMin, yHeightMax, fillValue);
        step = 0;
    }
}
