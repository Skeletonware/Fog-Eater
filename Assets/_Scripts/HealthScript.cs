using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    static float maxHealth;
    static float curHealth;

    static GameObject blood;

    // min is ~270 // now it's like 227???
    
    static float y;
    static float s;

    private void Start()
    {
        blood = transform.Find("HealthBlood").gameObject;        
    }

    public static void SetMax(float max)
    {
        maxHealth = max;
        curHealth = max;
    }

    private void Update()
    {
        float e = curHealth / maxHealth;
        s = Mathf.Lerp(blood.transform.localScale.y, e, 0.05f); //0.003f
        y = -(227 - (s * 227));   //Mathf.MoveTowards(blood.transform.localPosition.y, -(227 - (s * 227)), 0.005f);

        blood.transform.localScale = new Vector3(1, s, 1);
        blood.transform.localPosition = new Vector3(0, y, 0);
    }

    public static void UpdateHealth(float amt)
    {
        curHealth = amt;
    }
}
