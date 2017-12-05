using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFogger : MonoBehaviour {

    public Sprite fog;

    public static ScreenFogger Instance;

    List<GameObject> screenFogs = new List<GameObject>();

    GameObject blackScreen;

	void Start () {
        Instance = this;
        blackScreen = transform.Find("BlackImage").gameObject;
	}
	
	void Update () {
		
	}

    public IEnumerator FogScreen()
    {
        for (int i = 0; i < 125; i++)
        {
            CreateNewFog();
            CreateNewFog();
            CreateNewFog();
            CreateNewFog();            

            if (i > 100 && blackScreen.GetComponent<Image>().color.a < 1)
            {
                float fadeSpd = 0.075f;
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, blackScreen.GetComponent<Image>().color.a + fadeSpd);
            }

            yield return new WaitForSeconds(0);
        }
        yield break;
    }

    public IEnumerator ShowScreen()
    {        
        while (screenFogs.Count > 0)
        {
            GameObject fog = screenFogs[0];
            StartCoroutine(FadeFog(fog));

            fog = screenFogs[0];
            StartCoroutine(FadeFog(fog));

            fog = screenFogs[0];
            StartCoroutine(FadeFog(fog));

            fog = screenFogs[0];
            StartCoroutine(FadeFog(fog));

            fog = screenFogs[0];
            StartCoroutine(FadeFog(fog));



            if (blackScreen.GetComponent<Image>().color.a > 0) {
                float fadeSpd = 0.075f;
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, blackScreen.GetComponent<Image>().color.a - fadeSpd);
            }
            yield return new WaitForEndOfFrame();
        }

    }

    public IEnumerator FadeFog(GameObject fog)
    {
        screenFogs.Remove(fog);
        while (fog.GetComponent<Image>().color.a > 0)
        {
            float fadeSpd = 0.2f;
            fog.GetComponent<Image>().color = new Color(1, 1, 1, fog.GetComponent<Image>().color.a - fadeSpd);
            yield return new WaitForEndOfFrame();
        }
        Destroy(fog);
    }

    void CreateNewFog()
    {
        GameObject fog = new GameObject("ScreenFog");
        fog.transform.parent = transform;
        fog.AddComponent<Image>().sprite = this.fog;
        fog.transform.localScale = new Vector3(1, 1, 1);

        float x = Random.Range(transform.GetComponent<RectTransform>().rect.xMin, transform.GetComponent<RectTransform>().rect.xMax);
        float y = Random.Range(transform.GetComponent<RectTransform>().rect.yMin, transform.GetComponent<RectTransform>().rect.yMax);

        float angle = Random.Range(0, 360);

        fog.transform.localPosition = new Vector3(x, y, 0);
        fog.transform.localEulerAngles = new Vector3(0, 0, angle);
        screenFogs.Add(fog);
    }
}
