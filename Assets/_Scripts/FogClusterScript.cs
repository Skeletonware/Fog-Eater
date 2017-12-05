using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogClusterScript : MonoBehaviour {

    bool generated = false;

    public List<GameObject> currentNodes = new List<GameObject>();

    public GameObject fogNodePrefab;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {

    }

    public void RemoveFog(GameObject fog)
    {
        currentNodes.Remove(fog);
    }

    void GenerateFog()
    {
        float width = transform.localScale.x / 2;
        float height = transform.localScale.x / 2;
        for (float x = -width + 2; x < width; x += 4f)
        {
            for (float y = height - 2; y > -height; y -= 4f)
            {
                GameObject fogNode = Instantiate(fogNodePrefab);
                fogNode.transform.parent = GameObject.Find("FogParent").transform;

                fogNode.GetComponent<FogNodeScript>().cluster = this;

                Vector3 pos = transform.position;

                fogNode.transform.position = new Vector3(pos.x + x, pos.y + y, 0);

                currentNodes.Add(fogNode);
            }
        }
    }

    void HideFog()
    {
        foreach (GameObject fog in new List<GameObject>(currentNodes))
        {
            if (fog != null)
                fog.SetActive(false);

        }
    }

    void UnhideFog()
    {
        foreach (GameObject fog in new List<GameObject>(currentNodes))
        {
            if (fog != null)
                fog.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OnScreenDetector")
        {
            if (!generated)
            {
                GenerateFog();
                generated = true;
            }
            else
            {
                UnhideFog();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OnScreenDetector")
        {
            HideFog();
        }
    }
}
