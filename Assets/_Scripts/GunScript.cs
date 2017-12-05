using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    Coroutine fireCo;

    float reloadSpeed;

    public GameObject bulletPrefab;

    Vector3 origPos;

    bool doneShooting = true;


	void Start () {
        origPos = transform.localPosition;
	}
	
	void Update () {

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Forest.Paused)
            return;

        LookAtMouse();

        if (Input.GetButtonDown("Fire1") && doneShooting)
        {
            if (Player.Script.bullets > 0)
            {
                DemonNotifier.AddRadius(4f);
                doneShooting = false;
                fireCo = StartCoroutine(Fire());
            } else
            {
                GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/click"));
                // click
            }
        }

    }

    void ToggleOff()
    {
        if (fireCo != null)
            StopCoroutine(fireCo);
        transform.localPosition = origPos;
        doneShooting = true;
    }

    IEnumerator Fire()
    {
        Player.Script.bullets--;
        BulletUI.RemoveBullet();
        ShootBullets();        
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Shoot"));

        transform.localPosition -= transform.TransformDirection(new Vector3(0, 0.3f, 0));
        int tick = 0;

        while (Vector3.Distance(transform.localPosition, new Vector3(origPos.x, origPos.y, transform.localPosition.z)) > 0.001f)
        {
            while (Forest.Paused)
                yield return null;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(origPos.x, origPos.y, transform.localPosition.z), 0.00875f);

            tick++;
            if (tick > 100)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        doneShooting = true;

    }

    void ShootBullets()
    {
        int bltAmt = 3;

        Vector3 currentRot = transform.Find("Sprite").eulerAngles;
        currentRot.z -= 5;


        for (int i = 0; i < bltAmt; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.Find("Sprite"));
            bullet.GetComponent<BulletScript>().SetSender(transform.parent.gameObject);
            bullet.transform.localPosition = new Vector3(0.5f, 0, 0);
            bullet.transform.parent = null;
            bullet.transform.eulerAngles = currentRot;
            bullet.GetComponent<BulletScript>().velocity = GameObject.Find("Player").GetComponent<Player>().velocity;
            currentRot.z += 5;
        }
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
