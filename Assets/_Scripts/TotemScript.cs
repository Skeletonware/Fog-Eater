using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemScript : MonoBehaviour {

    Color blue = new Color32(130, 224, 255, 255);
    Color green = new Color32(130, 255, 153, 255);
    Color red = new Color32(255, 130, 130, 255);
    Color purple = new Color32(200, 130, 255, 255);


    public static int activatedTotems = 0;

    Coroutine lightCo;

    Color myColor;

    public GameObject kirgan;
    public GameObject alter;


    bool lit = false;
    bool needsKirganKilled = false;

    public TotemColor color;

    public List<GameObject> alters = new List<GameObject>();
    public List<GameObject> alterBeams = new List<GameObject>();

    void Awake () {
        if (color == TotemColor.White)
            myColor = Color.white;
        if (color == TotemColor.Red)
            myColor = red;
        if (color == TotemColor.Green)
            myColor = green;
        if (color == TotemColor.Blue)
            myColor = blue;
        if (color == TotemColor.Purple)
            myColor = purple;

        if (kirgan != null)
        {
            needsKirganKilled = true;
        }
    }
	
	void Update () {
		
	}

    void Activate()
    {
        if (lit)
            return;

        if (needsKirganKilled && kirgan != null)
        {
            HelpText.Say(Random.Range(0, 100) < 50 ? "Can't activate it yet!" : "Gotta kill that thing first!");
            return;
        }

        if (color == TotemColor.White && activatedTotems < 4)
        {
            if (!HelpText.AlreadySaid(HelpText.p_mainTotem))
            {
                HelpText.Say(HelpText.p_mainTotem);
                HelpText.Say(HelpText.p_findTotems);
            }
            return;
        } else
        {
            if (activatedTotems == 0)
            {
                HelpText.Say("Alright! One down, three to go.");
                HelpText.Say("These smaller totems won't hurt the creatures, but the Enlightener will.");
            }
            if (activatedTotems == 1)
                HelpText.Say("Two down! I think I can hear more of the creatures out there...");
            if (activatedTotems == 2)
            {
                HelpText.Say("There we go. One more totem.");
            }
            if (activatedTotems == 3)
            {
                HelpText.Say("Done. Now to just activate the Enlightner.");
                HelpText.Say("I'd better be careful though; I think I can hear some of those big monsters out there.");
                Player.Script.totemSpawn = Player.Script.transform.position;
            }
            if (activatedTotems == 4)
            {
                HelpText.Say("Yes!!! See ya later, evil beasts!");
                HelpText.Say("Now I can finally get some rest.");
            }
        }

        MonsterManager.MakeItWorse();
        GameObject.Find("Compass").GetComponent<CompassScript>().totemList.Remove(gameObject);
        if (color == TotemColor.White)
        {
            Forest.GameWon = true;
            Forest.Sound.PlayOneShot(Resources.Load<AudioClip>("Sounds/Enlightenment2"));
            foreach (GameObject alter in alters)
            {
                alter.SendMessage("Activate");
                alterBeams.Add(alter.GetComponent<AlterScript>().Enlighten());
            }
        }
        else
            Forest.Sound.PlayOneShot(Resources.Load<AudioClip>("Sounds/totemSound2"));


        activatedTotems++;

        Player.Script.SetHealth(Player.Script.healthMax);

        lightCo = StartCoroutine(LightUp());
        lit = true;
    }

    IEnumerator LightUp()
    {
        if (alter != null)
            alter.SendMessage("Activate");

        GameObject totemBeam = new GameObject("TotemBeam");
        totemBeam.transform.parent = transform;
        totemBeam.transform.localPosition = new Vector3(0, 0, 0);
        SpriteRenderer sr = totemBeam.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Environment";
        sr.sortingOrder = 1;
        sr.sprite = Resources.Load<Sprite>("Totems/TotemBeam");
        sr.color = myColor;

        for (int i = 0; i < 10; i++)
        {
            GameObject beam = new GameObject("Beam");
            beam.transform.parent = totemBeam.transform;
            beam.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Totems/TotemBeamStraight");
            beam.GetComponent<SpriteRenderer>().sortingLayerName = "TopEnvironment";
            beam.GetComponent<SpriteRenderer>().sortingOrder = 1;
            beam.GetComponent<SpriteRenderer>().color = myColor;

            if (i > 4)
            {
                beam.GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, 1 - ((i - 5) * 0.2f));
            }

            beam.transform.localPosition = new Vector3(0, (2 * i) + 2, 0);
        }

        GameObject fogEater = new GameObject("TotemFogEater");
        fogEater.transform.parent = transform;
        fogEater.transform.position = transform.position;
        if (color == TotemColor.White)
            fogEater.tag = "FogDestroyer";
        else
            fogEater.tag = "FogSweeper";
        fogEater.layer = 2;
        
        CircleCollider2D circ = fogEater.AddComponent<CircleCollider2D>();
        circ.isTrigger = true;

        circ.radius = 1;

        float s = transform.Find("Background").localScale.x;

        totemBeam.transform.localScale = new Vector3(0, s, s);

        while (totemBeam.transform.localScale.x < s)
        {
            float x = Mathf.MoveTowards(totemBeam.transform.localScale.x, s, 0.025f);
            totemBeam.transform.localScale = new Vector3(x, s, s);

            foreach (GameObject aBeam in alterBeams)
            {
                aBeam.transform.localScale = new Vector3(x, s, s);
            }

            yield return new WaitForEndOfFrame();
        }

        float rad = 50;
        if (color == TotemColor.White)
            rad = 20;


        while (circ.radius < rad)
        {
            circ.radius += 0.04f;
            yield return new WaitForEndOfFrame();
        }


        if (color == TotemColor.White)
        {
            GameObject.Find("Demons").SetActive(false);
            yield return StartCoroutine(LevelManager.Whiter.WhiteScreen());
            yield return new WaitForSeconds(1);
            WinText.Say("You win!");

            yield return new WaitForSeconds(3);

            WinText.Say("Thanks for playing!\n-@skeletonware");
            yield return new WaitForSeconds(10);
            WinText.FadeText();
            yield return new WaitForSeconds(3);

            LevelManager.gameWon = true;
            LevelManager.LoadLevel(1);
            // load main menu
        }

        yield break;
    }


    void MenuLight()
    {
        if (alter != null)
            alter.SendMessage("Activate");

        if (color == TotemColor.White && !LevelManager.gameWon)
            return;

        GameObject totemBeam = new GameObject("TotemBeam");
        totemBeam.transform.parent = transform;
        totemBeam.transform.localPosition = new Vector3(0, 0, 0);
        SpriteRenderer sr = totemBeam.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Environment";
        sr.sortingOrder = 1;
        sr.sprite = Resources.Load<Sprite>("Totems/TotemBeam");
        sr.color = myColor;

        for (int i = 0; i < 10; i++)
        {
            GameObject beam = new GameObject("Beam");
            beam.transform.parent = totemBeam.transform;
            beam.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Totems/TotemBeamStraight");
            beam.GetComponent<SpriteRenderer>().sortingLayerName = "TopEnvironment";
            beam.GetComponent<SpriteRenderer>().sortingOrder = 1;
            beam.GetComponent<SpriteRenderer>().color = myColor;

            if (i > 4)
            {
                beam.GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, 1 - ((i - 5) * 0.2f));
            }

            beam.transform.localPosition = new Vector3(0, (2 * i) + 2, 0);
        }

        float s = transform.Find("Background").localScale.x;

        //totemBeam.transform.localScale = new Vector3(0, s, s);

        totemBeam.transform.localScale = new Vector3(s, s, s);


    }

    public void MenuBreak()
    {
        if ((!LevelManager.gameWon && color != TotemColor.White) || LevelManager.gameWon)
        {
            StartCoroutine(Break());
        }
    }

    public void MenuFix()
    {
        
        StartCoroutine(Fix());
        
    }

    IEnumerator Break()
    {
        


        GameObject myBeam = transform.Find("TotemBeam").gameObject;


        int len = Random.Range(80, 100);

        Color c;

        for (int i = 0; i < len; i++)
        {
            if (Random.Range(0, 100) < i)
            {
                c = myBeam.GetComponent<SpriteRenderer>().color;
                c.a = 0;
                myBeam.GetComponent<SpriteRenderer>().color = c;
                foreach (Transform beam in myBeam.transform)
                {
                    beam.GetComponent<SpriteRenderer>().color = c;
                }
            }
            else
            {
                c = myBeam.GetComponent<SpriteRenderer>().color;
                c.a = 1;
                myBeam.GetComponent<SpriteRenderer>().color = c;
                foreach (Transform beam in myBeam.transform)
                {
                    beam.GetComponent<SpriteRenderer>().color = c;
                }
            }
            yield return new WaitForEndOfFrame();
        }

        c = myBeam.GetComponent<SpriteRenderer>().color;
        c.a = 0;
        myBeam.GetComponent<SpriteRenderer>().color = c;
        foreach (Transform beam in myBeam.transform)
        {
            beam.GetComponent<SpriteRenderer>().color = c;
        }
    }


    IEnumerator Fix()
    {
        GameObject myBeam = transform.Find("TotemBeam").gameObject;

        int len = Random.Range(80, 100);

        Color c;

        for (int i = 0; i < len; i++)
        {
            if (Random.Range(0, 100) < i)
            {
                c = myBeam.GetComponent<SpriteRenderer>().color;
                c.a = 1;
                myBeam.GetComponent<SpriteRenderer>().color = c;
                foreach (Transform beam in myBeam.transform)
                {
                    beam.GetComponent<SpriteRenderer>().color = c;
                }
            }
            else
            {
                c = myBeam.GetComponent<SpriteRenderer>().color;
                c.a = 0;
                myBeam.GetComponent<SpriteRenderer>().color = c;
                foreach (Transform beam in myBeam.transform)
                {
                    beam.GetComponent<SpriteRenderer>().color = c;
                }
            }
            yield return new WaitForEndOfFrame();
        }

        c = myBeam.GetComponent<SpriteRenderer>().color;
        c.a = 1;
        myBeam.GetComponent<SpriteRenderer>().color = c;
        foreach (Transform beam in myBeam.transform)
        {
            beam.GetComponent<SpriteRenderer>().color = c;
        }
    }


    public enum TotemColor
    {
        Red,
        Blue,
        Green,
        Purple,
        White
    }

    private void OnDrawGizmos()
    {

        if (color == TotemColor.White)
            myColor = Color.white;
        if (color == TotemColor.Red)
            myColor = red;
        if (color == TotemColor.Green)
            myColor = green;
        if (color == TotemColor.Blue)
            myColor = blue;
        if (color == TotemColor.Purple)
            myColor = purple;

        Gizmos.color = myColor;
        float s = 0.75f;
        Gizmos.DrawWireCube(transform.position, new Vector3(s, s, s));
        s *= 0.8f;
        Gizmos.DrawWireCube(transform.position, new Vector3(s, s, s));
        s *= 0.8f;
        Gizmos.DrawWireCube(transform.position, new Vector3(s, s, s));
    }






}
