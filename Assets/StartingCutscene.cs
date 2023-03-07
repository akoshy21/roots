using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 


public class StartingCutscene : MonoBehaviour
{
   


    public LineRenderer line;
    public float lineEnd;
    public float step;
    
    public GameObject[] gameObjToInitialize;

    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.acornGroundHit, this.transform.position);
        StartCoroutine(ExtendLine());
    }

    private IEnumerator ExtendLine()
    {
        Vector3 pos = line.GetPosition(1);
        
        while (pos.y > lineEnd)
        {
            // Debug.Log("extending " + pos.y);
            pos.y += step;
            line.SetPosition(1, pos);
            yield return new WaitForSeconds(0.1f);
        }
        
        foreach (GameObject o in gameObjToInitialize)
        {
            o.SetConditionalActive(true);
        }
        
        Camera.main.GetComponent<CameraController>().CheckY(Vector3.zero);
        
        PlantManager.Instance.StartGame();

        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
