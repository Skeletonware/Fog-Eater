using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpText : MonoBehaviour {

    public static string p_start = "Crap. The void hasn't come here for ages.";
    public static string p_start2 = "Time to get to work, I guess.";
    public static string p_mainTotem = "I can't activate the main Enlightener until I've activated the other totems.";
    public static string p_findTotems = "I haven't seen them in a while, but my compass should tell me where they are. [LSHIFT]";
    public static string p_broom = "My handy-dandy broom should make quick work of this stuff. [TAB]";

    public static List<string> alreadySaid = new List<string>();

    public static List<string> talkQueue = new List<string>();

    bool talking = false;

    Coroutine sayCo;
    public float talkSpeed = 0;
    public Text texter;

    static HelpText ht;

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
        if (!talkQueue.Contains(text))
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
            while (Forest.Paused)
                yield return null;
            texter.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.025f);
            //yield return new WaitForEndOfFrame();
        }

        
        yield return new WaitForSeconds(time / 25);
        while (Forest.Paused)
            yield return null;
        texter.text = "";

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




}
