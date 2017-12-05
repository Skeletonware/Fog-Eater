using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDisablerScript : MonoBehaviour {

	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	void Update () {
		
	}
}
