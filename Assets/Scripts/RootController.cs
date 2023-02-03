using System.Collections;
using UnityEngine;

public class RootController: MonoBehaviour
{
    private Coroutine drawing;
    public GameObject rootPrefab;
    public GameObject rootParent;

    void StartRoot()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }

        drawing = StartCoroutine(DrawRoot());
    }

    void FinishLine()
    {
        StopCoroutine(drwaing);
    }

    IEnumerator DrawLine()
    {
        GameObject go = Instantiate(rootPrefab, rootParent.transform);
        LineRenderer line = go.GetComponent<LineRenderer>();
    }

    private void StopCoroutine(Coroutine coroutine)
    {
        throw new System.NotImplementedException();
    }
}