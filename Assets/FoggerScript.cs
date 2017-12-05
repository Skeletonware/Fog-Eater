using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoggerScript : MonoBehaviour {

    public static FoggerScript Instance;

	void Start () {
		
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


	}
	
	void Update () {
		
	}
}
