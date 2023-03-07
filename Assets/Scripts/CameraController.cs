using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float _lowestY = 1;
    public float camSpeed = 1;

    public float topY = 3.18f;
    
    private Vector3 _endPos;
    private Vector3 _origPos;

    private float _step = 0;

    public float LowestY => _lowestY;

    private bool _gameEnd = false;

    public GameObject menuUI, scraper;

    public GameObject smallTree, bigTree,acorn;

    // TODO: SCROLL UP TO THE TOP OF THE DIRT ON GAME-OVER SO YOU CAN SEE YOUR ROOT STRUCTURE (ALMOST AN ARTISTIC END)
    // TODO: OPENING CUTSCENE FROM ACORN

    public void CheckY(Vector3 pos)
    {
        // Debug.Log("check y " + _lowestY + pos.y);

        if (pos.y < _lowestY)
        {
            _lowestY = pos.y;
            _origPos = transform.position;
            _endPos = new Vector3(_origPos.x, _lowestY, -10);
            _step = 0;

            ItemSpawner.Instance.MoveDown();

            if (_lowestY < -15)
            {
                bigTree.SetConditionalActive(true);
                smallTree.SetConditionalActive(false);
                acorn.SetConditionalActive(false);
            }
            else if(_lowestY < -3)
            {
                smallTree.SetConditionalActive(true);
                acorn.SetConditionalActive(false);
            }
            
            if(_lowestY < -3.8)
                scraper.SetConditionalActive(true);
        }
    }

    public void Update()
    {
        _step += Time.deltaTime * camSpeed;
        if (_step > 1)
        {
            _step = 1;
            
            if(_gameEnd)
                menuUI.SetConditionalActive(true);
        }

        if (Vector3.Distance(_origPos, _endPos) > 0.25f)
        {
            transform.position = Vector3.Lerp(_origPos, _endPos, _step);
        }  
    }

    public void ScrollToTop()
    {
        _gameEnd = true;
        _origPos = transform.position;
        _endPos = new Vector3(_origPos.x, topY, -10);
        _step = 0;
    }
}
