using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowards : MonoBehaviour {

    public GameObject target;


	void Start () {
        if (target == null)
            target = GameObject.Find("Player");
	}

    void Update() {

        if (target.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
	}
}
