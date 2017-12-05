using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWhiter : MonoBehaviour {

    public static ScreenWhiter Instance;

    void Start () {
        Instance = this;
	}
	
	void Update () {
		
	}

    public IEnumerator WhiteScreen()
    {
        GameObject white = transform.Find("WhiteImage").gameObject;
        float fadeSpd = 0.05f;

        while (white.GetComponent<Image>().color.a < 1)
        {
            white.GetComponent<Image>().color = new Color(1, 1, 1, white.GetComponent<Image>().color.a + fadeSpd);

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator UnwhiteScreen()
    {
        GameObject white = transform.Find("WhiteImage").gameObject;
        float fadeSpd = 0.05f;

        while (white.GetComponent<Image>().color.a > 0)
        {
            white.GetComponent<Image>().color = new Color(1, 1, 1, white.GetComponent<Image>().color.a - fadeSpd);

            yield return new WaitForEndOfFrame();
        }
    }


}
