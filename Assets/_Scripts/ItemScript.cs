using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    public Item.Type type;

    int amt = 1;

    private void Start()
    {
        if (type == Item.Type.Bullet)
        {
            amt = Random.Range(1, 4);
        }
    }

    void Pickup()
    {
        amt--;
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/itemPickup"), transform.position);
        if (amt <= 0)
        {
            Destroy(gameObject);
        }
    }
}
