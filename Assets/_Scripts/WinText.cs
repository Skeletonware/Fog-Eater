using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour {

    public static List<string> alreadySaid = new List<string>();

    public static List<string> talkQueue = new List<string>();

    bool talking = false;

    Coroutine sayCo;
    public float talkSpeed = 0;
    public Text texter;

    static WinText ht;

    public static bool AlreadySaid(string text)
    {
        if (alreadySaid.Contains(text))
            return true;
        else
            return false;
    }

    public static void SayIndefinitely(string text)
    {
        if (ht.talking)
            return;
        ht.texter.text = text;
    }

    public static void Shush()
    {
        if (ht.talking)
            return;
        ht.texter.text = "";
    }

    private void Start()
    {
        texter = GetComponent<Text>();
        ht = this;
    }

    public static void Say(string text)
    {
        ht.SaySomething(text);
    }

    public void SaySomething(string text)
    {
        alreadySaid.Add(text);
        if (talking)
        {            
            talkQueue.Add(text);
        }
        else
        {
            sayCo = StartCoroutine(Talk(text));
        }
    }

    IEnumerator Talk(string text)
    {
        float time = text.Length;        
        talking = true;

        for (int i = 0; i <= text.Length; i++)
        {
            texter.text = text.Substring(0, i);
            yield return new WaitForSeconds(talkSpeed);
            //yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time / 25);
        //texter.text = "";

        if (talkQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.05f);
            string nextSaying = talkQueue[0];
            talkQueue.RemoveAt(0);            
            yield return StartCoroutine(Talk(nextSaying));

        } else
        {            
            talking = false;
        }

    }

    public static void FadeText()
    {
        ht.StartCoroutine(ht.Fade());
    }

    public IEnumerator Fade()
    {        
        float fadeSpd = 0.05f;

        while (texter.color.a > 0)
        {
            texter.color = new Color(0, 0, 0, texter.color.a - fadeSpd);

            yield return new WaitForEndOfFrame();
        }
    }




}
