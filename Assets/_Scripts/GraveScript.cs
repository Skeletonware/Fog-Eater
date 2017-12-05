using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveScript : MonoBehaviour {

	void Start () {
        foreach (Transform grave in transform)
        {
            grave.transform.localScale = new Vector3(Random.Range(0, 100) < 50 ? -1 : 1, 1, 1);
            int rand = Random.Range(1, 4);
            grave.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Graves/grave" + rand.ToString());
        }
    }
	
	void Update () {
		
	}
}
