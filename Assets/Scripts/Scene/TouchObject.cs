﻿using UnityEngine;
using UnityEngine.UI;

public class TouchObject : MonoBehaviour
{
    [SerializeField] private bool hasSound = false;
    [SerializeField] private bool hasName = false;
    [SerializeField] private bool scaleWithoutAnim = false;
    [SerializeField] private float scaleR = .1f;

    private Animator animator = null;
    private AudioSource sound = null;
    private GameObject obj = null;
    private Text text = null;

    Vector3 scaleS;
    float xScale = 0;
    float yScale = 0;

    private Touch touch;

    private void Start()
    {
        scaleS = transform.localScale;
        xScale = scaleS.x;
        yScale = scaleS.y;

    }
    bool scale = false;
    void SetRayCast(RaycastHit2D hitTouch)
    {
        if (hitTouch)
        {
            //if(hitTouch.collider.CompareTag("Difference"))
            //{
            //    obj = hitTouch.collider.transform.gameObject as GameObject;

            //    if (obj.GetComponent<SpriteRenderer>().color.a == 0)
            //        Parent.GetComponent<WasUnitComplete>().SetCountOfDifference +=  1;
            //    obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 255);
            //}
            if (hasSound && (transform == hitTouch.collider.transform) && (sound == null || !sound.isPlaying))
            {
                if (hasName && text == null)
                    text = GameObject.FindGameObjectWithTag("ObjectsName").gameObject.GetComponent<Text>() as Text;

                obj = hitTouch.collider.transform.gameObject as GameObject;

                try
                {
                    sound = obj.GetComponent<AudioSource>() as AudioSource;
                    sound.Play();
                }
                catch { sound = new AudioSource(); }
                try
                {
                    animator = obj.GetComponent<Animator>() as Animator;
                }
                catch { }
                if (!scaleWithoutAnim)
                    animator.SetBool("zoom", true);
                else
                    scale = true;
            }
        }
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitTouch = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);

            SetRayCast(hitTouch);
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            RaycastHit2D hitTouch = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(touch.deltaPosition).x, Camera.main.ScreenToWorldPoint(touch.deltaPosition).y), Vector2.zero, 0);

            SetRayCast(hitTouch);
        }


        if (animator != null) {
            if (sound.isPlaying)
            {
                if (!scaleWithoutAnim)
                    animator.SetBool("zoom", true);
                else
                    scale = true;
                if(hasName)
                    text.text = obj.name;
                GetComponent<SpriteRenderer>().sortingLayerName = "Selected";
            }
            else
            {
                sound.Stop();
                if(!scaleWithoutAnim)
                    animator.SetBool("zoom", false);
                else
                    scale = false;

                GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                if (hasName && text.text == obj.name)
                    text.text = null;
            }
        }

        if(scale && scaleWithoutAnim)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(xScale  + (xScale * scaleR), yScale + (yScale * scaleR)), .05f);
        }else if(!scale && scaleWithoutAnim)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleS, .05f);
        }
    }
}
