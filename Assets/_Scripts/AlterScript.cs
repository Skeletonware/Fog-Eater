using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterScript : MonoBehaviour {

    Color blue = new Color32(130, 224, 255, 255);
    Color green = new Color32(130, 255, 153, 255);
    Color red = new Color32(255, 130, 130, 255);
    Color purple = new Color32(200, 130, 255, 255);

    
    Color myColor;

    bool lit = false;

    public TotemScript.TotemColor color;

    GameObject alterBeam;

    void Start()
    {
        if (color == TotemScript.TotemColor.White)
            myColor = Color.white;
        if (color == TotemScript.TotemColor.Red)
            myColor = red;
        if (color == TotemScript.TotemColor.Green)
            myColor = green;
        if (color == TotemScript.TotemColor.Blue)
            myColor = blue;
        if (color == TotemScript.TotemColor.Purple)
            myColor = purple;
    }

    void Activate()
    {
        GameObject alterBeam = new GameObject("AlterBeam");
        alterBeam.transform.parent = transform;
        alterBeam.transform.localPosition = new Vector3(0, 0, 0);
        SpriteRenderer sr = alterBeam.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Environment";
        sr.sortingOrder = 1;
        sr.sprite = Resources.Load<Sprite>("Totems/AlterBeam");
        sr.color = myColor;
    }

    public GameObject Enlighten()
    {
        GameObject newAB = new GameObject("AlterBeam2");
        newAB.transform.parent = transform;
        newAB.transform.localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < 10; i++)
        {
            GameObject beam = new GameObject("Beam");
            beam.transform.parent = newAB.transform;
            beam.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Totems/TotemBeamStraight");
            beam.GetComponent<SpriteRenderer>().sortingLayerName = "TopEnvironment";
            beam.GetComponent<SpriteRenderer>().sortingOrder = 1;
            beam.GetComponent<SpriteRenderer>().color = myColor;

            if (i > 4)
            {
                beam.GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, 1 - ((i - 5) * 0.2f));
            }

            beam.transform.localPosition = new Vector3(0, (2 * i) + 1f, 0);
        }

        return newAB;
    }

    void Update () {
		
	}
}
