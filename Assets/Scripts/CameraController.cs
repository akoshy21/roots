using System;
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

    public void CheckY(Vector3 pos)
    {
        // Debug.Log("check y " + _lowestY + pos.y);

        if (pos.y < _lowestY && PlantManager.GAME_ACTIVE)
        {
            _lowestY = pos.y;
            _origPos = transform.position;
            _endPos = new Vector3(_origPos.x, _lowestY, -10);
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

        }
    }

    public void Update()
    {
        _step += Time.deltaTime * camStep;
        if (_step > 1)
        {
            _step = 1;

            if (_gameEnd)
                menuUI.SetConditionalActive(true);
        }

        if (Vector3.Distance(_origPos, _endPos) > 0.25f)
        {
            if (PlantManager.Instance)
                transform.position = Vector3.Lerp(_origPos, _endPos, _step);
            else
                transform.position = Vector3.MoveTowards(transform.position, _endPos, camSpeed);
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