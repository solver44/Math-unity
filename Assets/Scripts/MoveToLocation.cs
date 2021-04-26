﻿using System.Collections;
using UnityEngine;

public class MoveToLocation : MonoBehaviour
{
    [SerializeField]
    private float speedMove = 10; //Default 10
    [SerializeField]
    private float locationX = 0;
    [SerializeField]
    private float locationY = 1;
    [SerializeField]
    private bool isMoveToward = false;
    [SerializeField]
    private float waitForSecond = 0f;

    private bool gottaFinish = false;
    public void SetLocationX(float x, bool finish) { locationX = x; stop = false; gottaFinish = finish; }
    public void SetLocationY(float y, bool finish) { locationY = y; stop = false; gottaFinish = finish; }

    private bool stop = true;

    public bool FinishMove = false;

    [Header("Additional")]
    public bool isWaitFinishAnother = false;
    public MoveToLocation[] OtherObjects = null;

    private void Start()
    {
        StartCoroutine(waitForSec());
    }
    void Update()
    {
        if (stop == false && ((this.transform.localPosition.x > locationX + .01f || this.transform.localPosition.x < locationX - .01f) || (this.transform.localPosition.y > locationY + .01f || this.transform.localPosition.y < locationY - .01f)))
        {
            if (!isMoveToward)
                this.transform.localPosition = Vector2.Lerp(this.transform.localPosition, new Vector2(locationX, locationY), speedMove * Time.fixedDeltaTime);
            else
                this.transform.localPosition = Vector2.MoveTowards(this.transform.localPosition, new Vector2(locationX, locationY), speedMove * Time.fixedUnscaledDeltaTime);
        }
        else if (!stop)
        {
            this.transform.localPosition = new Vector2(locationX, locationY);
            stop = true;
        }

        if (gottaFinish && stop)
            FinishMove = true;
    }

    private IEnumerator waitForSec()
    {
        stop = true;
        yield return new WaitForSeconds(waitForSecond);
        stop = false;
    }
}