﻿using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Transform placeObject = null;
    private Animator placeObjectAnim = null;
    public Transform GameObjects;
    [SerializeField] private bool killMe = false;
    [SerializeField] private bool anyLocation = false;
    [SerializeField] private float rangeX = 3f;
    [SerializeField] private float rangeY = 1f;


    private Vector2 firstPosition;
    public static bool locked = false;
    private float deltaX, deltaY;
    private Vector3 mousePosition;
    private Animator anim;

    private Touch touch;

    private SpriteRenderer render = null;
    private Transform currentTransform = null;
    private void Awake()
    {
        anim = GetComponent<Animator>() as Animator;
        render = GetComponent<SpriteRenderer>() as SpriteRenderer;
        placeObjectAnim = placeObject.GetComponent<Animator>() as Animator;
        currentTransform = GetComponent<Transform>() as Transform;
    }
 
    private void OnMouseDown()
    {
        if(firstPosition != null)
            firstPosition = GetComponent<Transform>().position;
        if (!locked)
        {
            this.GetComponent<SpriteRenderer>().sortingOrder += 1;
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }
    private void OnMouseDrag()
    {
        if (!locked)
        {
            if(anyLocation)
                firstPosition = GetComponent<Transform>().position;
            anim.SetBool("zoom", true);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
        }
    }
    bool toLocalPos = false;
    private void OnMouseUp()
    {
        if (!locked)
        {
            anim.SetBool("zoom", false);
            render.sortingOrder -= 1;
            if (Mathf.Abs(transform.position.x - placeObject.position.x) <= rangeX && 
                Mathf.Abs(transform.position.y - placeObject.position.y) <= rangeY)
            {
                anim.SetTrigger("destroy");
                if (killMe)
                    placeObjectAnim.SetTrigger("destroy");
                StartCoroutine(makeInActive());
            }
            else
            {
                toLocalPos = true;
                transform.position = Vector2.Lerp(transform.position, firstPosition, 2f);
            }
        }
    }
    private IEnumerator makeInActive()
    {
        yield return new WaitForSeconds(1);
        //this.transform.gameObject.SetActive(false);
        Destroy(this.transform.gameObject);
        if(killMe)
            Destroy(placeObject.transform.gameObject);
    }

    void Update()
    {
        //if (GameObjects.position.x < .1f && GameObjects.position.x > -.1f)
        //{
        //    locked = true;
        //}
        //else
        //    locked = false;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(touch.deltaPosition).x, Camera.main.ScreenToWorldPoint(touch.deltaPosition).y), Vector2.zero, 0);
            if (hit && transform.name == hit.collider.transform.name)
            {
                if(touch.phase == TouchPhase.Began)
                {
                    if (firstPosition != null)
                        firstPosition = currentTransform.position;
                    if (!locked)
                    {
                        render.sortingOrder += 1;
                        deltaX = Camera.main.ScreenToWorldPoint(touch.deltaPosition).x - transform.position.x;
                        deltaY = Camera.main.ScreenToWorldPoint(touch.deltaPosition).y - transform.position.y;
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (!locked)
                    {
                        if (anyLocation)
                            firstPosition = currentTransform.position;
                        anim.SetBool("zoom", true);
                        mousePosition = Camera.main.ScreenToWorldPoint(touch.deltaPosition);
                        transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
                    }
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    if (!locked)
                    {
                        anim.SetBool("zoom", false);
                        render.sortingOrder -= 1;
                        if (Mathf.Abs(transform.position.x - placeObject.position.x) <= rangeX &&
                            Mathf.Abs(transform.position.y - placeObject.position.y) <= rangeY)
                        {
                            anim.SetTrigger("destroy");
                            if (killMe)
                                placeObjectAnim.SetTrigger("destroy");
                            StartCoroutine(makeInActive());
                        }
                        else
                        {
                            toLocalPos = true;
                            transform.position = Vector2.Lerp(transform.position, firstPosition, 2f);
                        }
                    }
                }

            }
        }
    }
}