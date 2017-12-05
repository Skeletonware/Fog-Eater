using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static ScreenFogger Fogger;
    public static ScreenWhiter Whiter;
    public static LevelManager Instance;
    public static bool gameWon = false;

    

    bool started = false;


	void Start () {
        Application.targetFrameRate = 60;
        if (LevelManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }        
        DontDestroyOnLoad(Fogger);
        Fogger = GameObject.Find("FogCanvas").transform.Find("ScreenFogger").GetComponent<ScreenFogger>();
        Whiter = GameObject.Find("FogCanvas").transform.Find("ScreenWhiter").GetComponent<ScreenWhiter>();

        LightTotems();
    }
	
    void LightTotems()
    {
        foreach (Transform totem in GameObject.Find("MenuTotems").transform)
        {
            totem.SendMessage("MenuLight");
        }
    }

	void Update () {
		
	}

    public static void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
        if (index == 1)
        {            
            Instance.StartCoroutine(Instance.WinAndMenu());
            Instance.started = false;
        }
    }

    public void Begin()
    {
        
        print("Beginning...");
        print(started);
        if (started)
            return;

        

        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/beginSound"));

        started = true;
        print("Here'a we go!!!");
        StartCoroutine(StartGame());
    }

    IEnumerator WinAndMenu()
    {
        yield return new WaitForSeconds(0.01f);

        foreach (Transform totem in GameObject.Find("MenuTotems").transform)
        {
            totem.SendMessage("MenuLight");
        }

        yield return StartCoroutine(Whiter.UnwhiteScreen());
    }

    IEnumerator StartGame()
    {
        // screen shake
        // sound
        // totems fade
        // fogscreen
        // load game

        GameObject MenuCanvas = GameObject.Find("MenuCanvas");

        while (MenuCanvas.GetComponent<CanvasGroup>().alpha > 0)
        {
            MenuCanvas.GetComponent<CanvasGroup>().alpha = MenuCanvas.GetComponent<CanvasGroup>().alpha - 0.05f;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);

        // play sound
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/explosionSound"));

        yield return StartCoroutine(new Shake().ShakeObject(GameObject.Find("Main Camera"), 100, 40));

        foreach (Transform totem in GameObject.Find("MenuTotems").transform)
        {
            totem.SendMessage("MenuBreak");
        }


        yield return new WaitForSeconds(2);

        gameWon = false;

        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/fogFade"));
        yield return StartCoroutine(LevelManager.Fogger.FogScreen()); //GameObject.Find("ScreenFogger").transform.GetComponent<ScreenFogger>().

        LoadLevel(2);
        Forest.Paused = true;

        yield return new WaitForSeconds(0.1f);

        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/fogFadeAway"));
        yield return StartCoroutine(LevelManager.Fogger.ShowScreen());

        Forest.Paused = false;
        started = false;

        yield break;
    }
}
