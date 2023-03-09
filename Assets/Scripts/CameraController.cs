using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float _lowestY = 1;
    public float camStep = 0.4f;
    public float camSpeed = 1;

    public float topY = 3.18f;

    private Vector3 _endPos;
    private Vector3 _origPos;

    private float _step = 0;

    public float LowestY => _lowestY;

    private bool _gameEnd = false;

    public GameObject menuUI;

    public GameObject smallTree, bigTree, acorn;

    public TMP_Text meterText;

    private bool checkForDeath = true;

    public void CheckY(Vector3 pos)
    {
        Debug.Log("check y " + _lowestY + pos.y);

        if (pos.y < _lowestY && PlantManager.GAME_ACTIVE)
        {
            _lowestY = pos.y;
            _origPos = transform.position;
            _endPos = new Vector3(_origPos.x, _lowestY - 2f, -10);
            _step = 0;

            ItemSpawner.Instance.MoveDown();

            if (_lowestY < -30)
            {
                bigTree.SetConditionalActive(true);
                smallTree.SetConditionalActive(false);
                acorn.SetConditionalActive(false);
            }
            else if (_lowestY < -10)
            {
                smallTree.SetConditionalActive(true);
                acorn.SetConditionalActive(false);
            }

            int cm = (int) (Mathf.Abs(_lowestY) + 1);


            meterText.text = cm.ToString() + "m";
        }
    }

    public void Update()
    {
        if (_gameEnd && Vector3.Distance(transform.position,_endPos) < 0.25f)
        {
            Debug.Log("end game");
            _step = 1;
            StartCoroutine(DelayMenu());
        }
        else if(Vector3.Distance(_origPos, _endPos) > 0.25f)
        {
            if (_gameEnd == false)
            {
                _step += Time.deltaTime * camStep;
                if (_step > 1)
                {
                    _step = 1;
                }
                
                transform.position = Vector3.Lerp(_origPos, _endPos, _step);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _endPos, camSpeed);
            }
        }
        else if (_gameEnd == false && checkForDeath && PlantManager.GAME_ACTIVE)
        {
            checkForDeath = false;
            PlantManager.Instance.CheckForDeath();
        }
    }

    IEnumerator DelayMenu()
    {
        yield return new WaitForSeconds(0.25f);
        menuUI.SetConditionalActive(true);
    }

    public void ScrollToTop()
    {
        _gameEnd = true;
        _origPos = transform.position;
        _endPos = new Vector3(_origPos.x, topY, -10);
        _step = 0;
    }
}