using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDetector : MonoBehaviour {

    public static List<GameObject> nearbyFogs = new List<GameObject>();
    public static List<GameObject> nearbyDemons = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fog")
            nearbyFogs.Add(collision.gameObject);
        if (collision.gameObject.tag == "Demon")
            nearbyDemons.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fog")
            nearbyFogs.Remove(collision.gameObject);
        if (collision.gameObject.tag == "Demon")
            nearbyDemons.Remove(collision.gameObject);
    }
}
