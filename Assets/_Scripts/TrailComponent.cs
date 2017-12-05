using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailComponent : MonoBehaviour {

    public Sprite sprite;
    public float trailLength;
    public float trailDistance;
    public float trailOpacity;

    public float fadeAmt;

    float trailTick;

    List<GameObject> trails = new List<GameObject>();

    public void Init(Sprite sprite, float length, float distance, float opacity, float fadeAmt = 0.025f)
    {
        this.sprite = sprite;
        trailLength = length;
        trailDistance = distance;
        trailOpacity = opacity;

        this.fadeAmt = fadeAmt;

        trailTick = trailDistance;
    }

	void Start () {
		
	}
	
	void Update () {

        if (Forest.Paused)
            return;

        trailTick--;
        if (trailTick <= 0)
        {
            GameObject trail = new GameObject("Trail particle");
            trail.transform.position = transform.position;
            trail.transform.parent = GameObject.Find("TrailParent").transform;
            trail.AddComponent<SpriteRenderer>().sprite = sprite;
            trail.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, trailOpacity);
            trail.GetComponent<SpriteRenderer>().sortingLayerName = "Entities";
            trail.transform.localScale = transform.Find("Sprite").transform.localScale;
            StartCoroutine(DisTrail(trail));
            trailTick = trailDistance;

            trails.Add(trail);
        }
		
	}

    IEnumerator DisTrail(GameObject trailParticle)
    {
        while (Forest.Paused)
            yield return null;
        yield return new WaitForSeconds(trailLength);
        
        StartCoroutine(FadeTrailParticle(trailParticle));
    }

    IEnumerator FadeTrailParticle(GameObject particle)
    {
        SpriteRenderer sp = particle.GetComponent<SpriteRenderer>();
        while (Forest.Paused)
            yield return null;
        while (sp.color.a > 0)
        {
            while (Forest.Paused)
                yield return null;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - fadeAmt);
            yield return new WaitForEndOfFrame();
        }
        
        trails.Remove(particle);
        Destroy(particle);
    }

    private void OnDestroy()
    {
        foreach (GameObject tr in new List<GameObject>(trails))
        {
            Destroy(tr);
        }
    }
}
