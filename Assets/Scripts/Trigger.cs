using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); ;
    }
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Trigger  " + Int32.Parse(Regex.Match(collision.gameObject.name, @"\d+").Value));
        if (collision.gameObject.tag == "Tile")
        {
            gameManager.CheckHit(collision.gameObject);
            Destroy(gameObject);
        }
    }
}