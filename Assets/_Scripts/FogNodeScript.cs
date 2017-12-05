using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogNodeScript : MonoBehaviour {

    bool generated = false;
    bool hidden = true;

    List<GameObject> currentFogs = new List<GameObject>();

    public GameObject fogPrefab;
    public FogClusterScript cluster;

    Coroutine hideCo;
    Coroutine showCo;

    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;




	}
	
	void Update () {
        
	}
    
    public void RemoveFog(GameObject fog)
    {
        currentFogs.Remove(fog);
    }

    void GenerateFog()
    {   
        hidden = false;
        for (float x = -1; x < 2; x += 1.25f)
        {
            for (float y = 1; y > -2; y -= 1.25f)
            {
                GameObject fog = Instantiate(fogPrefab);
                fog.transform.parent = GameObject.Find("FogParent").transform;

                fog.GetComponent<FogScript>().node = this;

                Vector3 pos = transform.position;

                fog.transform.position = new Vector3(pos.x + x, pos.y + y, 0);

                currentFogs.Add(fog);
            }
        }
    }

    void HideFog()
    {        
        hidden = true;
        foreach (GameObject fog in new List<GameObject>(currentFogs))
        {
            fog.SetActive(false);
        }        
    }

    IEnumerator UnhideFog()
    {        
        hidden = false;
        int done = 0;
        int max = 10;
        foreach (GameObject fog in new List<GameObject>(currentFogs))
        {
            fog.SetActive(true);
            done++;
            if (done >= max)
            {
                done = 0;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void OnEnable()
    {
        Forest.activeFogNodes.Add(gameObject);
    }

    private void OnDisable()
    {
        Forest.activeFogNodes.Remove(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FogDisabler")
        {   
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "OnScreenDetector")
        {
            if (!generated)
            {
                GenerateFog();
                generated = true;
            } else
            {
                if (hideCo != null)
                    StopCoroutine(hideCo);
                if (showCo != null)
                    StopCoroutine(showCo);
                showCo = StartCoroutine(UnhideFog());
            }
        }

        if (collision.gameObject.tag == "FogSweeper" && hidden)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OnScreenDetector")
        {
            if (showCo != null)
                StopCoroutine(showCo);
            HideFog();
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject fog in new List<GameObject>(currentFogs))
        {
            Destroy(fog);
        }
    }




}
