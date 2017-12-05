using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomScript : MonoBehaviour {

    Coroutine broomCo;

    Vector3 origPos;

    float broomSpeed = 0.03f;

    bool doneSweeping = true;

	void Start () {
        origPos = transform.localPosition;
	}
	
	void Update () {

        if (Forest.Paused)
            return;
        LookAtMouse();

        if (Input.GetButton("Fire1") && doneSweeping)
        {
            doneSweeping = false;
            broomCo = StartCoroutine(Sweep());
        }


    }

    void ToggleOff()
    {
        doneSweeping = true;
        transform.localPosition = origPos;        
    }

    IEnumerator Sweep()
    {
        transform.localPosition += transform.TransformDirection(new Vector3(0, 0.5f, 0));
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Sweep"));
        yield return new WaitForEndOfFrame(); // this allows the broom to add the closest fogs to the fog list before sweeping them

        foreach (GameObject demon in new List<GameObject>(FogDetector.nearbyDemons))
        {
            if (demon != null)
            {
                demon.SendMessage("TakeDamage", new Damage(20, transform.parent.gameObject, 3));
                broomSpeed = 0.0075f;
            }
            
        }

        foreach (GameObject fog in FogDetector.nearbyFogs)
        {
            fog.SendMessage("GetSweeped");
        }

        int tick = 0;

        while (Vector3.Distance(transform.localPosition, new Vector3(origPos.x, origPos.y, transform.localPosition.z)) > 0.0001f)
        {
            while (Forest.Paused)
                yield return null;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(origPos.x, origPos.y, transform.localPosition.z), broomSpeed);

            tick++;
            if (tick > 100)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        broomSpeed = 0.03f;

        doneSweeping = true;
        yield break;
    }

    void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        if (Input.mousePosition.x < Screen.width / 2)
        {
            transform.Find("Sprite").localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.Find("Sprite").localScale = new Vector3(1, 1, 1);
        }
    }
}
