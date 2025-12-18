using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameObjectHide : MonoBehaviour
{
    public Tilemap map;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            map.color = new Color(1, 1, 1, 0.95f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            map.color = new Color(1, 1, 1, 1);
        }
    }
}
