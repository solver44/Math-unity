﻿using System.Collections;
using UnityEngine;

public class ShowChildAlpha : MonoBehaviour
{
    [SerializeField] private bool makeShow = true;
    public GameObject[] ChildAnswer = null;
    [SerializeField] private float visibleTime = 2f;

    [Header("Additional")]
    [SerializeField] private float rangeOfTransition = 0.5f;
    [SerializeField] private int sizeOfIndex = 18;
    public GameObject Parent = null;
    public bool IsWaitUntilClosing = true;
    public bool WhenClickThenClose = false;

    private bool isItStart = false;
    private bool isFirstTime = true;

    private float[] r, g, b;
    private void Start()
    {
        IsWaitUntilClosing = true;
        r = new float[sizeOfIndex];
        g = new float[sizeOfIndex];
        b = new float[sizeOfIndex];

        for (int i = 0; i < ChildAnswer.Length; i++)
        {
            if (ChildAnswer[i].transform.name == "RectangleAnswer" )
            { WhenClickThenClose = true; rangeOfTransition = 0.1f; }
            try
            {
        
                r[i] = ChildAnswer[i].GetComponent<TextMesh>().color.r;
                g[i] = ChildAnswer[i].GetComponent<TextMesh>().color.g;
                b[i] = ChildAnswer[i].GetComponent<TextMesh>().color.b;
            }
            catch
            {
                r[i] = ChildAnswer[i].GetComponent<SpriteRenderer>().color.r;
                g[i] = ChildAnswer[i].GetComponent<SpriteRenderer>().color.g;
                b[i] = ChildAnswer[i].GetComponent<SpriteRenderer>().color.b;
            }
        }
        
    }
    //private void OnMouseDown()
    //{
    //    //if (IsWaitUntilClosing)
    //    //{
    //        //if (!startAnim && currentIndex != 0 && !WhenClickThenClose)
    //        //{
    //            //currentIndex = 0;
    //            //startAnim = true;
    //        //}
    //        //if(WhenClickThenClose && startAnim)
    //        //{

    //        //}
    //    //}
    //    //else
    //    //{
    //        //currentIndex = 0;
    //        //startAnim = true;
    //    //}

    //}
    private bool startAnim = false;
    private Touch touch;

    void SetRayCast(RaycastHit2D hitTouch)
    {
        if (hitTouch)
        {
            if (hitTouch.collider.transform == transform) {

                if (IsWaitUntilClosing)
                { if (!startAnim && currentIndex != 0)
                    {
                        currentIndex = 0;
                        startAnim = true;
                }}else{
                        currentIndex = 0;
                        startAnim = true;
                }

                if (WhenClickThenClose && isItStart)
                {
                    for (int i = 0; i < ChildAnswer.Length; i++)
                    {
                        StartCoroutine(makeAlphaHideAndShow(0.0f, 1.0f, i, false));
                    }
                    currentIndex = -1;
                    startAnim = false;
                    isFirstTime = true;
                    isItStart = false;
                }
            }
            
        }
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            RaycastHit2D hitTouch = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(touch.deltaPosition).x, Camera.main.ScreenToWorldPoint(touch.deltaPosition).y), Vector2.zero, 0);
            SetRayCast(hitTouch);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitTouch = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
            SetRayCast(hitTouch);
        }

        if (currentIndex > -1 && startAnim )
        {
            countIndex();

            if (currentIndex >= ChildAnswer.Length)
            {
                currentIndex = -1;
                startAnim = false;
                isItStart = false;
                isFirstTime = true;
            }
        }
    }

    private void countIndex()
    {
        //if (!isFirstTime && rangeOfTransition != 0)
        //    yield return new WaitForSeconds(rangeOfTransition);
        //else
        //{
        //    yield return new WaitForSeconds(0);
        //}

        if (currentIndex > -1 && startAnim && isStartIt)
        {
            makeShowData();
            currentIndex++;
        }
    }

    private bool isStartIt = true;
    private int currentIndex = -1;
    private void makeShowData()
    {
        if (makeShow && isStartIt)
        {
            isStartIt = false;
            StartCoroutine(makeAlphaShow(currentIndex));
        }
    }
    float alpha = 0.0f;
    private IEnumerator makeAlphaShow(int index)
    {
        if (!isFirstTime && rangeOfTransition != 0)
            yield return new WaitForSeconds(rangeOfTransition);
        else if(rangeOfTransition != 0)
        {
            yield return new WaitForSeconds(0);
            isFirstTime = false;
        }

        try
        {
            alpha = ChildAnswer[index].GetComponent<TextMesh>().color.a;
        }
        catch
        {
            alpha = ChildAnswer[index].GetComponent<SpriteRenderer>().color.a;
        }
        if ((alpha == 1f && !WhenClickThenClose))
        {
            startAnim = false;
            yield return 0;
            StopCoroutine(makeAlphaShow(currentIndex));
        }

        StartCoroutine(makeAlphaHideAndShow(1.0f, 1.0f, index, false));
        isItStart = true;
        isStartIt = true;
        if (Parent != null)
        {
            Parent.GetComponent<WasUnitComplete>().SetCountOfDifference++;
        }
        if (visibleTime != 0 && !WhenClickThenClose)
        {
            yield return new WaitForSeconds(visibleTime);
            yield return StartCoroutine(makeAlphaHideAndShow(0.0f, 1.0f, index, false));
            isItStart = false;
        }
    }

    private IEnumerator makeAlphaHideAndShow(float aValue, float aTime, int index, bool isMakeFalse)
    {
        float alpha = 0.0f;
        try
        {
            alpha = ChildAnswer[index].GetComponent<TextMesh>().color.a;
        }
        catch
        {
            alpha = ChildAnswer[index].GetComponent<SpriteRenderer>().color.a;
        }
        
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / aTime)
        {
            Color newColor = new Color(r[index], g[index], b[index], Mathf.Lerp(alpha, aValue, i));
            try
            {
                ChildAnswer[index].GetComponent<TextMesh>().color = newColor;
            }
            catch
            {
                ChildAnswer[index].GetComponent<SpriteRenderer>().color = newColor;
            }
            yield return null;
        }
        if(isMakeFalse)
            isItStart = false;
    }

}
