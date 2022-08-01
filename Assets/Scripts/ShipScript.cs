using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ShipScript : MonoBehaviour
{
    GameManager gameManager;
    public List<GameObject> touchTiles = new List<GameObject>();
    public static int[] x4 = new int[4];
    public static int[] x3 = new int[3];
    public static int[] x31 = new int[3];
    public static int[] x23 = new int[2];
    public static int[] x24 = new int[2];
    public static int[] x25 = new int[2];
    public static int[] x1 = new int[1];
    public static int[] x11 = new int[1];
    public static int[] x12 = new int[1];
    public static int[] x13 = new int[1];
    public List<int[]> shipsCoordinates = new List<int[]> { x4, x3, x31, x23,x24,x25,x1,x11,x12,x13 };
    public float xOffset = 0;
    public float zOffset = 0;
    public float nextYRotation = -90;
    private GameObject clickedTile;
    public int shipSize;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); ;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            touchTiles.Add(collision.gameObject);
            int i = 0;
            switch (gameObject.name)
            {
                case "4x":
                    foreach (GameObject item in touchTiles)
                    {
                        x4[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "3x":
                    foreach (GameObject item in touchTiles)
                    {
                        x3[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "3x (1)":
                    foreach (GameObject item in touchTiles)
                    {
                        x31[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "2x (3)":
                    foreach (GameObject item in touchTiles)
                    {
                        x23[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "2x (4)":
                    foreach (GameObject item in touchTiles)
                    {
                        x24[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "2x (5)":
                    foreach (GameObject item in touchTiles)
                    {
                        x25[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "1x":
                    foreach (GameObject item in touchTiles)
                    {
                        x1[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "1x (1)":
                    foreach (GameObject item in touchTiles)
                    {
                        x11[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "1x (2)":
                    foreach (GameObject item in touchTiles)
                    {
                        x12[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
                case "1x (3)":
                    foreach (GameObject item in touchTiles)
                    {
                        x13[i] = Convert.ToInt32(item.name);
                        i++;
                    }
                    break;
            }
            if (gameManager.shipIndex == gameManager.ships.Length - 1)
            {
                gameManager.nextBtn.gameObject.SetActive(false);
                gameManager.btlBtn.gameObject.SetActive(true);
            }
        }
    }
    public void ClearTileList()
    {                
        touchTiles.Clear();
    }
    public Vector3 GetOffsetVec(Vector3 tilePos)
    {
        return new Vector3(tilePos.x + xOffset, 2, tilePos.z + zOffset);
    }

    public void RotateShip()
    {
        if (clickedTile == null) return;
        touchTiles.Clear();
        transform.localEulerAngles = new Vector3(0,nextYRotation, 0);
        nextYRotation += 90;
        (zOffset, xOffset) = (xOffset, zOffset);
        SetPosition(clickedTile.transform.position);
    }
    public void SetPosition (Vector3 Vec)
    {
        transform.position = new Vector3(Vec.x + xOffset, 2, Vec.z + zOffset);
    }
    public void SetClickedTile (GameObject tile)
    {
        ClearTileList();
        clickedTile = tile;
    }
    public Boolean OnGameBoard()
    {
        return touchTiles.Count == shipSize;
    }
}