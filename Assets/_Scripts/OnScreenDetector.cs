using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenDetector : MonoBehaviour {

    public static List<GameObject> onscreenObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Fog")
            onscreenObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Fog")
            onscreenObjects.Remove(collision.gameObject);
    }
}
