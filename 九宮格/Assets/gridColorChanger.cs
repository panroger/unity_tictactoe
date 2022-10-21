using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridColorChanger : MonoBehaviour
{
    [SerializeField] private Color hover;
    [SerializeField] private Color press;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        sr.color = hover;
        sr.enabled = true;
    }

    void OnMouseExit()
    {
        sr.enabled = false;
    }

    void OnMouseDown()
    {
        sr.color = press;
    }

    void OnMouseUp()
    {
        sr.color = hover;
        //do something
    }
}
