using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUI : MonoBehaviour {

    public List<GameObject> uiBullets = new List<GameObject>();

    public GameObject bulletPrefab;
    
    public static BulletUI instance;


    void Awake()
    {
        instance = this;
    }

    public static void AddBullets(int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            GameObject bullet = Instantiate(instance.bulletPrefab, instance.transform);
            //bullet.transform.parent = instance.transform;
            

            instance.uiBullets.Add(bullet);
        }
    }

    public static void SetBullets(int amt)
    {
        foreach (GameObject bullet in new List<GameObject>(instance.uiBullets))
        {
            Destroy(instance.uiBullets[0]);
            instance.uiBullets.RemoveAt(0);
        }

        AddBullets(amt);

    }

    public static void RemoveBullet()
    {
        Destroy(instance.uiBullets[0]);
        instance.uiBullets.RemoveAt(0);
    }
}
