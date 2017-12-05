using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonNotifier : MonoBehaviour {



    public float baseRad = 3;
    public float targetRad = 3;

    static CircleCollider2D collider;

    Player player;

    void Start () {
        player = Player.Script;
        collider = GetComponent<CircleCollider2D>();

    }
	
	void Update () {
        CalculateRadius();

        GetComponent<CircleCollider2D>().radius = Mathf.Lerp(GetComponent<CircleCollider2D>().radius, targetRad, 0.01f);




    }

    void CalculateRadius()
    {
        // based on:
        // health
        // bullets
        // gunshots

        float healthAdd = player.health / 30f;
        float bulletAdd = player.bullets / 10f;
        float gunshotAdd = player.justFired ? 4f : 0;

        targetRad = baseRad + healthAdd + bulletAdd;
        if (gunshotAdd != 0)
            GetComponent<CircleCollider2D>().radius = targetRad + gunshotAdd;
    }

    public static void AddRadius(float amt)
    {
        collider.radius += amt;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Demon")
        {
            collision.gameObject.SendMessage("SetTarget", transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
