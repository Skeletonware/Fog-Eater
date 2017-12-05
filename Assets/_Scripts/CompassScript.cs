using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour {

    GameObject closestTotem;
    GameObject player;
    GameObject needle;

    public List<GameObject> totemList = new List<GameObject>();

    Vector3 hiddenPos;
    public Vector3 shownPos;

    Vector3 targetPos;


    int searchTickMax = 10;
    int searchTick = 0;

    public float angle;

    void Start()
    {
        player = GameObject.Find("Player");
        needle = transform.Find("Needle").gameObject;

        hiddenPos = transform.localPosition;
        shownPos = transform.localPosition + shownPos;
        targetPos = hiddenPos;
    }
	
	void Update () {
        searchTick--;
        if (searchTick <= 0)
        {
            FindClosestTotem();
            searchTick = searchTickMax;
        }

        angle = AngleBetweenVector2(player.transform.position, closestTotem.transform.position) - 90;
        needle.transform.localEulerAngles = new Vector3(0, 0, angle);


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            targetPos = shownPos;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            targetPos = hiddenPos;
        }


        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.1f);
	}

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 difference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, difference) * sign;
    }

    public static void RemoveTotem(GameObject totem)
    {
        GameObject.Find("Compass").GetComponent<CompassScript>().totemList.Remove(totem);
        if (GameObject.Find("Compass").GetComponent<CompassScript>().totemList.Count == 0)
        {
            GameObject.Find("Compass").GetComponent<CompassScript>().closestTotem = GameObject.Find("WhiteTotem");
        }
    }

    void FindClosestTotem()
    {
        if (totemList.Count == 0)
        {
            closestTotem = GameObject.Find("WhiteTotem");
            return;
        }
        else
        {
            GameObject closest = totemList[0];
            foreach (GameObject totem in totemList)
            {
                if (Vector3.Distance(player.transform.position, totem.transform.position) < Vector3.Distance(player.transform.position, closest.transform.position))
                {
                    closest = totem;
                }
            }

            closestTotem = closest;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + shownPos;

        Gizmos.color = Color.green;

        float d = 0.3f;

        Gizmos.DrawLine(new Vector3(pos.x, pos.y + d, 0), new Vector3(pos.x, pos.y - d, 0));
        Gizmos.DrawLine(new Vector3(pos.x + d, pos.y, 0), new Vector3(pos.x - d, pos.y, 0));
    }
}
