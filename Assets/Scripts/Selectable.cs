using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    GameManager gameManager;
    public Color Old;
    private void Start()
    {
        Old = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Select()
    {
      // if (GetComponent<Renderer>().material.GetColor("_Color") != Color.white) Old = GetComponent<Renderer>().material.GetColor("_Color");
        //GetComponent<Renderer>().material.SetColor("BaseColorMap", Color.white);
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
    }
    public void Deselect()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor",Old);
    }
}