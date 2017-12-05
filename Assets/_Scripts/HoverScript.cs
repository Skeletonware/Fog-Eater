using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour {

    public Vector3 hoverOffset = new Vector3();

    public GameObject nearby;

    int moveTick = 0;


	void Start () {
		
	}
	
	void Update () {
        if (Forest.Paused)
            return;

        moveTick++;


        float y = Mathf.Sin(moveTick / 35f) / 10f;
        
        Vector3 position = hoverOffset;
        position.y += y;
        
        transform.localPosition = position;
	}

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + hoverOffset;

        Gizmos.color = Color.green;

        float d = 0.3f;

        Gizmos.DrawLine(new Vector3(pos.x, pos.y + d, 0), new Vector3(pos.x, pos.y - d, 0));
        Gizmos.DrawLine(new Vector3(pos.x + d, pos.y, 0), new Vector3(pos.x - d, pos.y, 0));
    }
}
