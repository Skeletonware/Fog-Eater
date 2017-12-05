using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkScript : MonoBehaviour {

    void Start () {
		
	}
	
	void Update () {
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!HelpText.AlreadySaid(HelpText.p_start)) {
                HelpText.Say(HelpText.p_start);
                HelpText.Say(HelpText.p_start2);
            }
        }
	}
}
