using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour {

	void Start () {

        foreach (Transform path in transform)
        {
            path.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 361) / 4);



        }



		
	}
	
	void Update () {
		
	}
}
