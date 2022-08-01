using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerRay : MonoBehaviour
{
    public static Selectable CurrentSelectable;
    private bool missileHit = false;
    Color[] hitColor = new Color[2];
    GameManager gameManager;
    MissileController missileController;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        missileController = GameObject.Find("MissileSpawn").GetComponent<MissileController>();
        if (CurrentSelectable && CurrentSelectable.GetComponent<Renderer>().material.GetColor("_Color") != Color.white) hitColor[0] = GetComponent<Renderer>().material.GetColor("_Color"); 
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Missile")) missileHit = true;
        else if (collision.gameObject.CompareTag("EnemyMissile")) CurrentSelectable.Select(); //смена цвета клетки при выборе клетки противником
    }
    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward*1000f, Color.yellow);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit))
        {
            Selectable selectable = hit.collider.gameObject.GetComponent<Selectable>();
            if (selectable)
            {
                if (CurrentSelectable && CurrentSelectable != selectable)
                {
                    CurrentSelectable.Deselect();
                }
                CurrentSelectable = selectable;
                selectable.Select();
                if (Input.GetMouseButton(0) && missileHit == false && hit.collider.gameObject)
                {
                    missileController.TargetTransform = hit.collider.gameObject.transform;
                    gameManager.TileClicked(hit.collider.gameObject);
                }

            }
            else // выделен объект без свойства Selectable
            {
                if (CurrentSelectable)
                {
                    CurrentSelectable.Deselect();
                    CurrentSelectable = null;
                }
            }
        }
        else
        {
            if (CurrentSelectable)
            {
                CurrentSelectable.Deselect();
                CurrentSelectable = null;
            }
        }        
}
} 