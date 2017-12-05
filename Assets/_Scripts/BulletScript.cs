using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    float speed = 25;

    public Vector3 velocity;

    GameObject hitEntity;
    GameObject sender;

    float damage = 15;

    public void SetSender(GameObject obj)
    {
        sender = obj;
    }
    
    void Update()
    {
        if (Forest.Paused)
            return;
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.right)); // + transform.TransformDirection(new Vector3(0, 0, speed))

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, 0.2f);

        if (hit.collider != null)
        {
            hitEntity = hit.collider.gameObject;
            Hit();
        }

    }

    void Hit()
    {
        hitEntity.SendMessage("TakeDamage", new Damage(damage, sender), SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + transform.TransformDirection(new Vector3(0, 0, speed)), transform.position + transform.TransformDirection(Vector3.right) * 2);
    }


}
