using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour {

    public GameObject fogNodes;
    public GameObject pausePanel;

    public static List<GameObject> nearbyFogs = new List<GameObject>();
    public static List<GameObject> activeFogNodes = new List<GameObject>();

    static List<List<GameObject>> fogLists = new List<List<GameObject>>();

    static List<GameObject> updateEveryFrame = new List<GameObject>();

    public static AudioSource Sound;

    public static bool GameWon = false;

    public static bool Paused = false;


    int addingList = 0;
    int updateList = 0;

    void Start()
    {
        Sound = GetComponent<AudioSource>();

        GenerateHouses();
        GenerateForests();
        GenerateBushes();
        GeneratePaths();


        fogLists.Add(new List<GameObject>());
        fogLists.Add(new List<GameObject>());
        fogLists.Add(new List<GameObject>());
        fogLists.Add(new List<GameObject>());
        fogNodes.SetActive(true);
    }

    void GenerateHouses()
    {

    }
    void GenerateForests()
    {

    }
    void GenerateBushes()
    {

    }
    void GeneratePaths()
    {

    }

    void TogglePause()
    {
        Paused = !Paused;
        pausePanel.SetActive(Paused);
    }




    public static void SetImportant(GameObject fog)
    {
        RemoveFog(fog);
        updateEveryFrame.Add(fog);
    }


    public static void AddFog(GameObject fog)
    {
        List<GameObject> listToAdd = fogLists[0];


        foreach (List<GameObject> list in fogLists)
        {
            if (list.Count < listToAdd.Count)
                listToAdd = list;
        }

        listToAdd.Add(fog);
    }

    public static void RemoveFog(GameObject fog)
    {
        if (updateEveryFrame.Contains(fog))
        {
            updateEveryFrame.Remove(fog);
            return;
        }


        foreach (List<GameObject> list in fogLists)
        {
            if (list.Contains(fog))
            {
                list.Remove(fog);
                break;
            }
        }

    }


	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (Paused)
            return;


		foreach (GameObject fog in new List<GameObject>(fogLists[updateList]))
        {
            if (fog == null)
                fogLists[updateList].Remove(fog);
            else
                fog.SendMessage("FogUpdate", SendMessageOptions.DontRequireReceiver);
        }
        updateList++;
        if (updateList > fogLists.Count - 1)
            updateList = 0;

        foreach (GameObject fog in new List<GameObject>(updateEveryFrame))
        {
            if (fog != null)
                fog.SendMessage("FogUpdate", SendMessageOptions.DontRequireReceiver);
        }
	}
}
