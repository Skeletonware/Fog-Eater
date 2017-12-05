using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour {

    Vector3 target;
    Vector3 origPos;

    Vector3 velocity;

    public bool sweeped = false;

    public float dist = 0;
    float speed = 0.02f;
    float os = 0.0175f;

    int movetick = 20;

    public FogNodeScript node;

    bool hitBySweeper = false;

	void Start () {
        origPos = transform.position;
        SetNextTarget();

        float s = 1f;

        transform.localScale = new Vector3(s, s, s);

        // by the way, this is definitely the worst way to implement a ton of objects into one scene. 
        // i've never really done it before, so this is what we got.
        Forest.AddFog(gameObject);
	}
	
	void FogUpdate () {
        if (!OnScreenDetector.onscreenObjects.Contains(gameObject))
        {
            if (hitBySweeper)
                Die();
            else
                return;
        }

        if (sweeped)
        {
            float opac = transform.GetComponent<SpriteRenderer>().color.a;

            if (opac <= 0)
            {
                Die();
            }

            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, opac - os);
        }
        
        Vector3 move = Vector3.MoveTowards(transform.position, target, speed);
        //Vector3 next = Vector3.Lerp(transform.position, target, speed);

        velocity.x += move.x * 0.025f;
        velocity.y += move.y * 0.025f;

        velocity.x = Mathf.Clamp(velocity.x, -0.01f, 0.01f);
        velocity.y = Mathf.Clamp(velocity.y, -0.01f, 0.01f);

        transform.position = move;
        
        movetick--;
        if ((movetick <= 0 || Vector3.Distance(transform.position, origPos) > 1) && !sweeped)
        {
            SetNextTarget();
            movetick = 20;
        }		
	}

    void SetNextTarget()
    {
        target = new Vector3((Random.Range(-100f, 100f) / 100) + origPos.x, (Random.Range(-100f, 100f) / 100) + origPos.y, 0);
    }

    void GetSweeped()
    {
        if (sweeped)
            return;

        if (Random.Range(0, 100) < 15)
            return;

        Forest.SetImportant(gameObject);
        sweeped = true;

        target = new Vector3((Random.Range(-400f, 400f) / 100) + origPos.x, (Random.Range(-400f, 400f) / 100) + origPos.y, 0);
        speed = 0.07f;
    }

    void GetSweepedAway()
    {
        if (sweeped)
            return;

        Forest.SetImportant(gameObject); // makes the fog run at 60 fps instead of 20 or whatever it is
        sweeped = true;

        target = new Vector3((Random.Range(-400f, 400f) / 100) + origPos.x, (Random.Range(-400f, 400f) / 100) + origPos.y, 0);
        speed = 0.1f;
    }

    private void OnEnable()
    {
        if (hitBySweeper)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "FogSweeper" || collision.gameObject.tag == "FogDestroyer")
        {
            hitBySweeper = true;

            GetSweepedAway();
        }

    }

    void Die()
    {
        if (hitBySweeper && Random.Range(0, 100) < 80)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/TotemSweep"), transform.position);
        }


        node.RemoveFog(gameObject);
        Forest.RemoveFog(gameObject);
        Destroy(gameObject);
    }
}
