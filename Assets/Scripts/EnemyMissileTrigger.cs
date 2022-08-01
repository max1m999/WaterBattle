using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyMissileTrigger : MonoBehaviour
{
    GameManager gameManager;
    EnemyScript enemyScript;
    public int collisionTile;
    void Start()
    {
        enemyScript = GameObject.Find("Enemy").GetComponent<EnemyScript>(); ;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); ;
    }
    public void OnTriggerEnter(Collider collision)//-------------
    {
        Debug.Log("EnemyMissileTrigger  tag:  " + Convert.ToString(collision.gameObject.tag));
        gameManager.EnemyHitPlayer(collisionTile);
        Destroy(gameObject);
    }
}