using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour {

    public static List<Sprite> PW_Side = new List<Sprite>();
    public static List<Sprite> PW_Down = new List<Sprite>();
    public static List<Sprite> PW_Up = new List<Sprite>();


    void Start () {

        PlayerAnims();
        

    }

    void PlayerAnims()
    {
        PW_Side = new List<Sprite>() {
            Resources.Load<Sprite>("Player/Player9"),
            Resources.Load<Sprite>("Player/Player10"),
            Resources.Load<Sprite>("Player/Player11"),
            Resources.Load<Sprite>("Player/Player12")
        };

        PW_Down = new List<Sprite>() {
            Resources.Load<Sprite>("Player/Player1"),
            Resources.Load<Sprite>("Player/Player2"),
            Resources.Load<Sprite>("Player/Player3"),
            Resources.Load<Sprite>("Player/Player4")
        };

        PW_Up = new List<Sprite>() {
            Resources.Load<Sprite>("Player/Player5"),
            Resources.Load<Sprite>("Player/Player6"),
            Resources.Load<Sprite>("Player/Player7"),
            Resources.Load<Sprite>("Player/Player8")
        };
    }
	
	void Update () {
		
	}
}
