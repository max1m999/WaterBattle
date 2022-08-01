using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    [Header("Player`s")]
    public GameObject[] ships;
    public GameObject[] Field1 = new GameObject[100];
    private bool playerTurn = true;
    private bool setupComplete = false;
    public int shipIndex = 0;
    private ShipScript shipScript;
    private List<GameObject> playerFires = new List<GameObject>();
    MissileController missileController;

    [Header("Enemy`s")]
    private bool enemyTurn = false;
    public EnemyScript enemyScript;
    public EnemyMissileTrigger enemyMissileTrigger;
    private List<int[]> enemyShips;

    [Header("Buttons")]
    public Button nextBtn;
    public Button btlBtn;
    public Button rotateBtn;
    public Button replayBtn;
    public Button exitBtn;

    [Header("Prefabs")]
    public GameObject Dock;
    public GameObject MainCamera;
    public GameObject enemyMissilePrefab;
    public GameObject MissilePrefab;
    public GameObject Fire;

    [Header("Interface")]
    public TMP_Text Text;
    public TMP_Text ShpsCntrsTitle;
    public TMP_Text EnmyShps;
    public TMP_Text PlrShps;
    private int shipsCount = 10;
    private int enemyShipsCount = 10;

    [Header("CameraMove")]
    public Vector3 CameraPos1;
    public Vector3 CameraPos2;
    public float cameraMoveDuration;

    bool bombed = false;
    Color Old;
    void Start()
    {
        Old = GameObject.Find("31").GetComponent<Selectable>().Old;
        missileController = GameObject.Find("MissileSpawn").GetComponent<MissileController>();
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        nextBtn.onClick.AddListener(() => NextShipClicked());
        rotateBtn.onClick.AddListener(() => RotateClicked());
        btlBtn.onClick.AddListener(() => BtlClicked());
        replayBtn.onClick.AddListener(() => ReplayClicked());
        exitBtn.onClick.AddListener(() => ExitClicked());
        enemyShips = enemyScript.PlaceEnemyShips();
        EnmyShps.text = enemyShipsCount.ToString();
        PlrShps.text = shipsCount.ToString();

    }

    void ExitClicked()
    {
        Application.Quit();
    }

    public void CheckHit(GameObject tile)
    {
        Debug.Log("CheckHit");
        int tileNum = Int32.Parse(Regex.Match(tile.name, @"\d+").Value);
        int hitCount = 0;
        foreach (int[] tileNumArray in enemyShips)
        {
            if (tileNumArray.Contains(tileNum))
            {
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if (tileNumArray[i] == tileNum)
                    {
                        tileNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (tileNumArray[i] == -5)
                    {
                        hitCount++;
                    }
                }
                if (hitCount == tileNumArray.Length)
                {
                    enemyShipsCount--;
                    Text.text = "Потопил!";
                    Instantiate(Fire, tile.transform.position, Quaternion.identity);
                    Destroy(tile.GetComponent<Selectable>());
                    tile.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                }
                else
                {
                    Text.text = "Попадание!";
                    Instantiate(Fire, tile.transform.position, Quaternion.identity);
                    Destroy(tile.GetComponent<Selectable>());
                    tile.GetComponent<Renderer>().material.SetColor("_BaseColor", Old);

                }
                break;
            }
        }
        if (hitCount == 0)
        {
            Text.text = "Промах";
            tile.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
            Destroy(tile.GetComponent<Selectable>());
        }
   Invoke("ChangeTurn", 2.0f);
    }
    public void EnemyHitPlayer(int tileNum)
    {
        int hitCount = 0;
        enemyScript.MissileHit(tileNum);
        GameObject tile = GameObject.Find(tileNum.ToString());
        Debug.Log("Found tile:   " + tileNum);        
        foreach (int[] tileNumArray in shipScript.shipsCoordinates)
        {
            if (tileNumArray.Contains(tileNum))
            {
                Debug.Log("CONTAINS");
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if (tileNumArray[i] == tileNum)
                    {
                        tileNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (tileNumArray[i] == -5)
                    {
                        hitCount++;
                    }                   
                }
                if (hitCount == tileNumArray.Length)
                {
                    shipsCount--;
                    Text.text = "Противник потопил корабль";
                    tile.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                    PlrShps.text = shipsCount.ToString();
                    enemyScript.SunkPlayer();
                    Instantiate(Fire, tile.transform.position, Quaternion.identity);
                    break;
                }
                else
                {
                    Instantiate(Fire, tile.transform.position, Quaternion.identity);
                    Text.text = "Противник подбил корабль";
                }
                break;
            }
        }
         if (hitCount == 0)
        {
            Text.text = "Промах";
            tile.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
            enemyScript.Aiming(tileNum);
        }
        Invoke("ChangeTurn", 3.0f);
    }

    void RotateClicked()
    {
        shipScript.RotateShip();
    }
    private void NextShipClicked()
    {
        if (!shipScript.OnGameBoard())
        {
            //Debug.Log("TouchTilesCount:  " + shipScript.touchTiles.Count);
            Text.text = "Недопустимое расположение корабля!";
        }
        else
        {
            Text.text = "Расположите свои корабли";            
            if (shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                shipScript = ships[shipIndex].GetComponent<ShipScript>();
            }            
        }
    }
    private void BtlClicked()
    {
        rotateBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        btlBtn.gameObject.SetActive(false);
        Dock.SetActive(false);
        PlrShps.gameObject.SetActive(true);
        EnmyShps.gameObject.SetActive(true);
        ShpsCntrsTitle.gameObject.SetActive(true);
        setupComplete = true;
        StartCoroutine(MoveCamera());
        Text.text = "Выберите клетку";
        foreach (GameObject tile in Field1)
        {
            Destroy(tile.GetComponent<Selectable>()); // выключить использование скрипта Selectable - сделать поле1 некликабельным
        }
        foreach (int[] tileNumArray in shipScript.shipsCoordinates)
        { 
            for (int i = 0; i<tileNumArray.Length; i++)
            {
                Debug.Log("our:  " + tileNumArray[i]);
            }
            Debug.Log("-----------------");
        }
    }
    public IEnumerator MoveCamera()
    {
        for (float i = 0.01f; i < cameraMoveDuration; i += 0.01f)
        {
            MainCamera.transform.position = Vector3.Lerp(CameraPos1, CameraPos2, i / cameraMoveDuration);
            yield return null;
        }
        (CameraPos2, CameraPos1) = (CameraPos1, CameraPos2);
    }
    public void TileClicked(GameObject tile)
    {
        if (setupComplete && playerTurn && !bombed)
        {
            missileController.Shot();
            bombed = true;
        }

        else if (!setupComplete)
        {
            ShipPlace(tile);
            shipScript.SetClickedTile(tile);
        }
    }
public void ChangeTurn()
    {
        if (enemyTurn == false)
        {
            playerTurn = false;
            enemyTurn = true;
            bombed = false;
            EnmyShps.text = enemyShipsCount.ToString();
            StartCoroutine(MoveCamera());
            Text.text = "Ход противника";
            enemyScript.AI();
            if (shipsCount == 0) GameOver("Поражение((");
        }
        else
        {
            playerTurn = true;
            enemyTurn = false;
            PlrShps.text = shipsCount.ToString();
            StartCoroutine(MoveCamera());
            Text.text = "Выберите клетку";
            if (enemyShipsCount == 0) GameOver("Победа))");

        }
    }  
    void GameOver(string winner)
    {
        Text.text = "GAME OVER " + winner;
        replayBtn.gameObject.SetActive(true);
    }
    void ReplayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShipPlace(GameObject tile)
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        Vector3 newVec = shipScript.GetOffsetVec(tile.transform.position);
        ships[shipIndex].transform.position = newVec;
    }
}