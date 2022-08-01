using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    char[] playerGrid;
    List<int> potentialHits = new List<int>();
    List<int> currentHits = new List<int>();
    private int guess;
    public GameObject EnemyMissilePrefab;
    GameManager gameManager;
    public EnemyMissileTrigger enemyMissileTrigger;
    public EnemyMissileScript enemyMissileScript;
    public List<int[]> PlaceEnemyShips()
    {        
        List<int[]> enemyShips = new List<int[]>
        {
            new int[]{-1, -1, -1, -1},
            new int[]{-1, -1, -1},
            new int[]{-1, -1, -1},
            new int[]{-1, -1},
            new int[]{-1, -1},
            new int[]{-1, -1},
            new int[]{-1},
            new int[]{-1},
            new int[]{-1},
            new int[]{-1}
        };
        int[] gridNumbers = Enumerable.Range(1, 100).ToArray();
        bool taken = true;
        foreach (int[] tileNumArray in enemyShips)
        {
            taken = true;
            while (taken)
            {
                taken = false;
                int shipNose = UnityEngine.Random.Range(0, 99);
                int rotateBool = UnityEngine.Random.Range(0, 2);
                int minusAmount = rotateBool == 0 ? 10 : 1;
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if ((shipNose - (minusAmount * i)) < 0 || gridNumbers[shipNose - i * minusAmount] < 0)
                    {
                        taken = true;
                        break;
                    }
                    else if (minusAmount == 1 && shipNose != ((shipNose - i * minusAmount)- 1))
                    {
                        taken = true;
                        break;
                    }
                }
                if (taken == false)
                {
                    for (int i = 0; i < tileNumArray.Length; i++)
                    {
                        tileNumArray[i] = gridNumbers[shipNose - i * minusAmount];
                        gridNumbers[shipNose - i * minusAmount] = -1;
                    }
                }
            }            
        }
        foreach (int[] numArray in enemyShips) //testing enemy ships places
        {
            string temp = "";
            for (int i = 0; i < numArray.Length; i++)
                temp += ", " + numArray[i];
            Debug.Log(temp);
        }
        return enemyShips;
    }
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerGrid = Enumerable.Repeat('n', 100).ToArray(); //n = попадания ещё не было
    }

    void Update()
    {
        
    }
    public void AI()
    {
        List<int> hitIndex = new List<int>();
        for (int i = 0; i < playerGrid.Length; i++)
        {
            if (playerGrid[i] == 'y') hitIndex.Add(i);
        }
         if (hitIndex.Count >= 1)
        {
            List<int> closeTiles = new List<int>();
            closeTiles.Add(1);
            closeTiles.Add(-1);
            closeTiles.Add(10);
            closeTiles.Add(-10);
            int index = Random.Range(0, closeTiles.Count);
            int possibleGuess = hitIndex[0] + closeTiles[index];
            while (!(possibleGuess >= 0 && possibleGuess < 100) || playerGrid[possibleGuess] != 'n' && closeTiles.Count > 0)
            {
                closeTiles.RemoveAt(index);
                index = Random.Range(0, closeTiles.Count);
                possibleGuess = hitIndex[0] + closeTiles[index];
                Debug.Log("Index:   " + index);
            }
            guess = possibleGuess;
        }
        else
        {
            int nextIndex = Random.Range(0, 100);
            while (playerGrid[nextIndex] != 'n') nextIndex = Random.Range(0, 100);
            guess = nextIndex;
            nextIndex = GuessAgainCheck(nextIndex);
            nextIndex = GuessAgainCheck(nextIndex);
            guess = nextIndex;
        }
        GameObject tile = GameObject.Find((guess).ToString());
        playerGrid[guess] = 'm';
        enemyMissileScript.SetTarget(guess);
        enemyMissileTrigger.collisionTile = guess;
        enemyMissileScript.TargetTransform = tile.transform;
        enemyMissileScript.Shot();
    }
    private int GuessAgainCheck(int nextIndex)
    {
        int newGuess = nextIndex;
        bool edgeCase = nextIndex < 10 || nextIndex > 89 || nextIndex %10 == 0 || nextIndex %10 == 9;
        bool nearGuess = false;
            if (nextIndex + 1 < 100) nearGuess = playerGrid[nextIndex + 1] != 'n';
            if (!nearGuess && nextIndex -1>0) nearGuess = playerGrid[nextIndex - 1] != 'n';
        if (!nearGuess && nextIndex + 10 < 100) nearGuess = playerGrid[nextIndex + 10] != 'n';
        if (!nearGuess && nextIndex - 10 > 0) nearGuess = playerGrid[nextIndex - 10] != 'n';
        if (edgeCase|| nearGuess) newGuess = Random.Range(0, 100);
        while (playerGrid[newGuess] !='n') newGuess = Random.Range(0, 100);
            return newGuess;

    }
    public void MissileHit(int hit)
    {
        playerGrid[guess] = 'y';
    }
    private void EndTurn()
    {
        gameManager.GetComponent<GameManager>().ChangeTurn();
    }
    public void SunkPlayer()
    {
        for (int i = 0; i < playerGrid.Length; i++)
        {
            if (playerGrid[i] == 'y') playerGrid[i] = 'x';
        }
    }
    public void Aiming(int miss)
    {
        Debug.Log("Aiming");
        if (currentHits.Count > 0 && currentHits[0] > miss)
        {
            foreach (int potential in potentialHits)
            {
                if (currentHits[0] > miss)
                {
                    if (potential < miss) potentialHits.Remove(potential);
                }
                else if (potential > miss) potentialHits.Remove(potential);
            }
        }
    }
}