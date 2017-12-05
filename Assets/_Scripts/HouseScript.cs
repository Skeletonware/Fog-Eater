using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour {

    GameObject top;

	void Start () {
        top = transform.Find("Top").gameObject;
        top.SetActive(true);
        transform.Find("Colliders").parent = transform.parent;
	}
	
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            top.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            top.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


}
