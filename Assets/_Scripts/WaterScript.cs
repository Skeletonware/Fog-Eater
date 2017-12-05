using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour {

    public Sprite sprite1;
    public Sprite sprite2;

    int time = 50;
    int max = 50;

	void Start () {
		
	}
	
	void Update () {

        SpriteRenderer spriter = GetComponent<SpriteRenderer>();

        time--;
		if (time <= 0)
        {
            if (spriter.sprite == sprite1)
                spriter.sprite = sprite2;
            else
                spriter.sprite = sprite1;

            time = max;
        }
	}
}
