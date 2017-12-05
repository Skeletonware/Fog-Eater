using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour {
    
    DemonType demon;

    Vector3 origPos;
    Vector3 targetPos;
    public Vector3 velocity;

    public GameObject bulletPrefab;
    
    public int wanderTick;
    public int wanderTickMax = 150;

    int stepTick;
    int stepTickMax = 50;

    Coroutine kickCo;

    TrailComponent trail;

    public DemonType.Type type = DemonType.Type.Null;

    public enum State
    {
        Idle,
        Chasing,
        Charging,
        Jumping,
        Running,
        Prowling
    }

    public State curState = State.Idle;

	void Start () {

        if (type == DemonType.Type.Null)
        {
            type = DemonType.Type.Dresh;
        }

        if (type == DemonType.Type.Kirgan)
            demon = new DemonType.Kirgan();
        else
            demon = new DemonType.Dresh();
        demon.Init(this);
        wanderTick = Random.Range(0, wanderTickMax);

        trail = gameObject.AddComponent<TrailComponent>();
        if (type == DemonType.Type.Dresh)
        {
            trail.Init(transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite, 0.3f, 10, 0.8f);
            MonsterManager.dreshes.Add(gameObject);
        }
        else if (type == DemonType.Type.Kirgan)
            trail.Init(transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite, 0.5f, 5, 1f);

        origPos = transform.position;
	}
	
	void Update () {

        if (Forest.Paused)
            return;

        if ((type == DemonType.Type.Dresh || TotemScript.activatedTotems > 3) && Vector3.Distance(transform.position, Player.Script.transform.position) > 40)
        {
            Die(false);
        }

        if (TotemScript.activatedTotems > 3 && Player.Script.dead)
        {
            Die(false);
        }

        if (demon.GetTarget() != null && kickCo == null)
        {           
            if (demon.GetTarget().GetComponent<Player>().dead)
            {
                demon.SetTarget(null);
                curState = State.Idle;
                return;
            }

            if (curState == State.Chasing)
            {
                float dist = Vector3.Distance(transform.position, demon.GetTarget().transform.position);
                if (dist > demon.attackDist)
                {
                    velocity = Vector3.Normalize(demon.GetTarget().transform.position - transform.position) * (demon.speed + 1f);
                }
                else
                {
                    demon.Attack();
                }
            }
            if (curState == State.Jumping)
            {
                demon.Attack();
                if (Vector3.Distance(transform.position, demon.GetTarget().transform.position) < demon.hitDistance)
                {
                    demon.GetTarget().SendMessage("TakeDamage", new Damage(demon.damage, gameObject));
                }
            }
            if (curState == State.Running || curState == State.Prowling)
            {
                demon.Run();
            }
        } else
        {            
            wanderTick--;
            if (wanderTick <= 0)
            {
                Wander();
                wanderTick = wanderTickMax;
            }
        }

        if (type == DemonType.Type.Kirgan && curState == State.Idle)
            CheckForCollisions();
        else if (type != DemonType.Type.Kirgan)
            CheckForCollisions();

        transform.position += velocity * Time.deltaTime;

        if (velocity.x != 0 || velocity.y != 0)
        {
            stepTick--;
            if (stepTick < 0)
            {
                PlayWalkSound();
                stepTick = stepTickMax;
            }
        }
        else
        {
            stepTick = 0;
        }


    }

    void SetTarget(GameObject obj)
    {

        if (obj == null)
        {
            if (Player.aggroList.Contains(gameObject))
                Player.aggroList.Remove(gameObject);
            return;
        }
        else if(TotemScript.activatedTotems < 4)
        {
            if (Player.aggroList.Count >= 2 && type == DemonType.Type.Dresh)
            {
                return;
            }
            else if (type == DemonType.Type.Dresh)
            {
                if (!Player.aggroList.Contains(gameObject))
                    Player.aggroList.Add(gameObject);
            }
        }

        PlaySound(demon.alertSound);
        curState = State.Chasing;
        stepTickMax = 35;

        //a.spatialBlend = 1;
        //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/dreshAlert"), transform.position, 1.5f);
        demon.SetTarget(obj);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource a = GetComponent<AudioSource>();
        a.clip = clip;

        if (clip != Resources.Load<AudioClip>("Sounds/demonWalk"))
        {
            a.pitch = 1 + (Random.Range(-10f, 10f) / 100f);
            a.spatialBlend = 0.25f;
            a.Play();
        }
        else
        {
            a = transform.Find("Stepper").GetComponent<AudioSource>();
            a.clip = clip;
            a.pitch = 1 + (Random.Range(-10f, 10f) / 100f);
            a.Play();
        }

    }

    public void PlayWalkSound()
    {
        AudioSource a = transform.Find("Stepper").GetComponent<AudioSource>();
        a.clip = demon.walkSound; //Resources.Load<AudioClip>("Sounds/demonWalk");
        
        a.pitch = 1 + (Random.Range(-10f, 10f) / 100f);
        a.Play();
    }

    public void Wander(bool walkForSure = false)
    {
        if (Vector3.Distance(transform.position, origPos) > 6.5f)
        {
            velocity = Vector3.Normalize(origPos - transform.position) * demon.speed;
        }
        else
        {
            if (Random.Range(0, 100) < 11 && walkForSure == false)
            {
                velocity.x = 0;
                velocity.y = 0;
            }
            else
            {
                velocity.x = (Random.Range(-100f, 100f) / 100f) * demon.speed;
                velocity.y = (Random.Range(-100f, 100f) / 100f) * demon.speed;
            }
        }
    }

    void CheckForCollisions()
    {
        Ray xRay = new Ray(transform.position, new Vector3(velocity.x, 0, 0)); // transform.TransformDirection(new Vector3(velocity.x, 0, 0)));
        Ray yRay = new Ray(transform.position, new Vector3(0, 0, velocity.y)); //transform.TransformDirection(new Vector3(0, 0, velocity.y)));
        RaycastHit2D hitX = Physics2D.Raycast(transform.position, new Vector2(velocity.x, 0), 0.5f, 1 << LayerMask.NameToLayer("Collidables"));
        RaycastHit2D hitY = Physics2D.Raycast(transform.position, new Vector2(0, velocity.y), 1f, 1 << LayerMask.NameToLayer("Collidables"));

        if (hitX.collider != null)
            velocity.x = 0;
        if (hitY.collider != null)
            velocity.y = 0;
    }

    void TakeDamage(Damage dmg)
    {
        if (kickCo == null)
            kickCo = StartCoroutine(Kick(dmg));

        PlaySound(demon.hurtSound);


        demon.health -= dmg.amount;
        if (demon.health <= 0)
        {
            Die();
        }
    }

    IEnumerator Kick(Damage dmg)
    {
        
        int kickTime = 40;
        Vector3 velocity = new Vector3();
        //velocity.y += 3f * dmg.kickMult;
        velocity.x = -Vector3.Normalize(dmg.source.transform.position - transform.position).x * 15f * dmg.kickMult;
        velocity.y = -Vector3.Normalize(dmg.source.transform.position - transform.position).y * 15f * dmg.kickMult;

        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
        
        for (int i = 0; i < kickTime; i++)
        {   
            velocity.x *= 0.9f;
            velocity.y *= 0.9f;

            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            CheckForCollisions();
            transform.position += velocity * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;

        kickCo = null;
    }

    void Die(bool killedByPlayer = true)
    {
        MonsterManager.dreshes.Remove(gameObject);

        if (killedByPlayer)
        {
            if (Random.Range(0, 100) < 50)
            {
                GameObject item = Instantiate(bulletPrefab);
                item.GetComponent<ItemScript>().type = Item.Type.Bullet;
                item.transform.position = transform.position;
            }
            AudioSource.PlayClipAtPoint(demon.dieSound, transform.position);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Player.aggroList.Contains(gameObject))
            Player.aggroList.Remove(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FogDestroyer")
        {
            Die();
        }
    }
}
