using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileScript : MonoBehaviour
{
    GameManager gameManager;
    EnemyScript enemyScript;
    private int targetTile = -1;
    public Transform SpawnTransform;
    public Transform TargetTransform;

    public float AngleInDegrees;

    float g = Physics.gravity.y;

    public GameObject Bullet;
    void Update()
    {
        SpawnTransform.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f);
    }
    public void Shot()
    {
        Vector3 fromTo = TargetTransform.position - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);


        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        GameObject newBullet = Instantiate(Bullet, SpawnTransform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = SpawnTransform.forward * v;
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyScript = GameObject.Find("Enemy").GetComponent<EnemyScript>();        
}
    public void SetTarget (int target)
    {
        targetTile = target;
    }
}