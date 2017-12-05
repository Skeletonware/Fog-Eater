using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    public static int maxMonsters = 8;
    public static List<GameObject> dreshes = new List<GameObject>();

    public GameObject dreshPrefab;
    public GameObject kirganPrefab;

    static int moreMonsterCount = 3;
    static int totemsRestored = 0;

    static bool spawnKirgans = false;


    void Start () {
		
	}
	
	void Update () {
        if (Forest.GameWon)
            return;
		
        if (dreshes.Count < maxMonsters && Random.Range(0, 500) < 10)
        {
            if (spawnKirgans && Random.Range(0, 100) < 50)
                GenerateRandomKirgan();
            else
                GenerateRandomDresh();
        }
	}

    public static void MakeItWorse()
    {
        maxMonsters += moreMonsterCount;
        moreMonsterCount++;
        totemsRestored++;

        if (totemsRestored > 3)
        {
            spawnKirgans = true;
        }

    }




    void GenerateRandomDresh()
    {
        //foreach (GameObject node in Forest.activeFogNodes)
        //{

        for (int i = 0; i < Forest.activeFogNodes.Count; i++)
        {
            GameObject node = Forest.activeFogNodes[Random.Range(0, Forest.activeFogNodes.Count - 1)];

            if (Random.Range(0, 100) < 3 && Vector3.Distance(node.transform.position, Player.Script.gameObject.transform.position) > 10)
            {
                GameObject dresh = Instantiate(dreshPrefab);
                dresh.transform.parent = GameObject.Find("Demons").transform;
                dresh.transform.position = node.transform.position;
                break;
            }            
        }
    }

    void GenerateRandomKirgan()
    {
        for (int i = 0; i < Forest.activeFogNodes.Count; i++)
        {
            GameObject node = Forest.activeFogNodes[Random.Range(0, Forest.activeFogNodes.Count - 1)];

            if (Random.Range(0, 100) < 3 && Vector3.Distance(node.transform.position, Player.Script.gameObject.transform.position) > 10)
            {
                GameObject dresh = Instantiate(kirganPrefab);
                dresh.transform.parent = GameObject.Find("Demons").transform;
                dresh.transform.position = node.transform.position;
                break;
            }
        }
    }
}
