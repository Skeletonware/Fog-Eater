using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Vector3 velocity;
    float speed = 0.5f;
    float maxSpeed = 5;

    Vector3 moveVector;

    GameObject Broom;
    GameObject Gun;

    List<GameObject> nearbyFogs = new List<GameObject>();
    List<GameObject> nearbyItems = new List<GameObject>();
    List<GameObject> nearbyActives = new List<GameObject>();

    public static List<GameObject> aggroList = new List<GameObject>();

    public static Player Script;

    int stepTick;
    int stepTickMax = 25;

    public float health = 100;
    public float healthMax = 100;

    public int bullets = 10;

    public bool justFired = false;
    
    Coroutine kickCo;

    SpriteAnimator animator;

    int posTrackerMax = 150;
    int posTracker = 0;

    List<Vector3> lastPos = new List<Vector3>();

    int stuckSayTick = 300;

    Coroutine dieCo;

    public bool dead = false;

    public Vector3 totemSpawn;


	void Start () {

        Broom = transform.Find("Broom").gameObject;
        Gun = transform.Find("Gun").gameObject;

        Script = this;

        HealthScript.SetMax(healthMax);
        BulletUI.AddBullets(bullets);
        animator = transform.Find("Sprite").GetComponent<SpriteAnimator>();
        animator.SetSpeed(10);
        
    }
	
	void Update () {

        if (Forest.Paused)
        {
            if (animator.isAnimating())
                animator.PauseAnimation();

            if (Input.GetKeyDown(KeyCode.Escape))
                animator.ContinueAnimation();
            return;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //TakeDamage(new Damage(1000, gameObject, 0));
        }


        if (!dead)
        {
            posTracker--;
            if (posTracker <= 0)
            {
                lastPos.Add(transform.position);
                posTracker = posTrackerMax;

                if (lastPos.Count > 30)
                {
                    lastPos.RemoveAt(0);
                }
            }

            stuckSayTick--;
            if (CheckForStuck() && stuckSayTick <= 0)
            {
                stuckSayTick = 300;
                HelpText.Say("I'm stuck! Mash [BACKSPACE]!!!");
            }

            justFired = false;
            if (Input.GetButtonDown("SwitchWeapons"))
            {
                SwitchWeapons();
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && CheckForStuck())
            {
                transform.position = lastPos[lastPos.Count - 1];
                lastPos.RemoveAt(lastPos.Count - 1);
            }

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                stepTick--;
                if (stepTick < 0)
                {
                    PlaySound(Resources.Load<AudioClip>("Sounds/step"), true);
                    stepTick = stepTickMax;
                }
            }
            else
            {
                stepTick = 0;
            }


            Animate();
            DetectItems();
            Move();
        }
	}

    void Animate()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 pos = transform.Find("Gun").localPosition;
            transform.Find("Gun").localPosition = new Vector3(pos.x, pos.y, 0.1f);
            transform.Find("Broom").localPosition = new Vector3(pos.x, pos.y, 0.1f);

            animator.SetAnimation(Animations.PW_Up);
            animator.StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 pos = transform.Find("Gun").localPosition;
            transform.Find("Gun").localPosition = new Vector3(pos.x, pos.y, -0.1f);
            transform.Find("Broom").localPosition = new Vector3(pos.x, pos.y, -0.1f);

            animator.SetAnimation(Animations.PW_Down);
            animator.StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKey(KeyCode.A) && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))))
        {
            animator.SetAnimation(Animations.PW_Side);
            transform.Find("Sprite").localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            animator.StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKey(KeyCode.D) && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))))
        {
            animator.SetAnimation(Animations.PW_Side);
            transform.Find("Sprite").localScale = new Vector3(1.5f, 1.5f, 1.5f);
            animator.StartAnimation();
        }

        if (animator.isAnimating() && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            animator.StopAnimation();
        }
    }

    void SwitchWeapons()
    {
        if (Gun.activeSelf == true)
        {
            Gun.SendMessage("ToggleOff");
            Gun.SetActive(false);
            Broom.SetActive(true);
        } else
        {
            Gun.SetActive(true);
            Broom.SendMessage("ToggleOff");
            Broom.SetActive(false);
        }
    }

    void Move()
    {
        if (kickCo == null)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                velocity.x -= speed;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                velocity.x += speed;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                velocity.y -= speed;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                velocity.y += speed;
            }

            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
            velocity.y = Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed);
        }


        velocity *= 0.9f;

        moveVector = velocity;

        CheckForCollisions();

        if (nearbyFogs.Count > 0)
            transform.position += velocity * Time.deltaTime * 0.25f;
        else
            transform.position += velocity * Time.deltaTime;
    }

    void DetectItems()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyItems.Count > 0)
        {
            PickupItem(nearbyItems[0]);            
        } else if (Input.GetKeyDown(KeyCode.E) && nearbyActives.Count > 0)
        {   
            nearbyActives[0].SendMessage("Activate");
        }
    }

    void CheckForCollisions()
    {
        Ray xRay = new Ray(transform.position, new Vector3(velocity.x, 0, 0)); // transform.TransformDirection(new Vector3(velocity.x, 0, 0)));
        Ray yRay = new Ray(transform.position, new Vector3(0, 0, velocity.y)); //transform.TransformDirection(new Vector3(0, 0, velocity.y)));
        RaycastHit2D hitX = Physics2D.Raycast(transform.position, new Vector2(velocity.x, 0), 0.5f, 1 << LayerMask.NameToLayer("Collidables"));
        RaycastHit2D hitY = Physics2D.Raycast(transform.position, new Vector2(0, velocity.y), 1f, 1 << LayerMask.NameToLayer("Collidables"));
        
        if (hitX.collider != null)
            velocity.x = -velocity.x * 0.3f;
        if (hitY.collider != null)
            velocity.y = -velocity.y * 0.3f;
    }

    bool CheckForStuck()
    {
        bool stuck = false;

        Ray xRay = new Ray(transform.position, new Vector3(velocity.x, 0, 0)); // transform.TransformDirection(new Vector3(velocity.x, 0, 0)));
        Ray yRay = new Ray(transform.position, new Vector3(0, 0, velocity.y)); //transform.TransformDirection(new Vector3(0, 0, velocity.y)));
        RaycastHit2D hitXLeft = Physics2D.Raycast(transform.position + new Vector3(-0.05f, 0, 0), new Vector2(-0.5f, 0), 0.5f, 1 << LayerMask.NameToLayer("Collidables"));
        RaycastHit2D hitXRight = Physics2D.Raycast(transform.position + new Vector3(0.05f, 0, 0), new Vector2(0.5f, 0), 0.5f, 1 << LayerMask.NameToLayer("Collidables"));
        RaycastHit2D hitYUp = Physics2D.Raycast(transform.position + new Vector3(0, 0.05f, 0), new Vector2(0, velocity.y + 0.5f), 1f, 1 << LayerMask.NameToLayer("Collidables"));
        RaycastHit2D hitYDown = Physics2D.Raycast(transform.position + new Vector3(0, -0.05f, 0), new Vector2(0, velocity.y - 0.5f), 1f, 1 << LayerMask.NameToLayer("Collidables"));

        if (hitXLeft.collider != null && hitXRight.collider != null && hitYDown.collider != null && hitYUp.collider != null)
        {
            stuck = true;
        }

        return stuck;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + ((transform.TransformDirection(moveVector))));
    }

    public static void PlaySound(AudioClip sound, bool random = false)
    {

        Player.Script.GetComponent<AudioSource>().clip = sound;

        if (random)
        {
            Script.GetComponent<AudioSource>().pitch = 1 + Random.Range(-10f, 10f) / 100;
        }


        Player.Script.GetComponent<AudioSource>().Play();
        //Player.Script.GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void PickupItem(GameObject item)
    {
        if (item.GetComponent<ItemScript>().type == Item.Type.Bullet && bullets < 20)
        {
            bullets++;
            BulletUI.AddBullets(1);
            nearbyItems[0].SendMessage("Pickup");
        }

        if (item.GetComponent<ItemScript>().type == Item.Type.Medkit && health < healthMax)
        {
            float newHealth = health + 20;

            if (newHealth > healthMax)
                newHealth = healthMax;

            SetHealth(newHealth);
            nearbyItems[0].SendMessage("Pickup");
        }
    }

    void TakeDamage(Damage dmg)
    {
        if (kickCo == null)
            kickCo = StartCoroutine(Kick(dmg));
        else
            return;

        SetHealth(health - dmg.amount);
        
        if (health <= 0)
        {
            dieCo = StartCoroutine(Die());
        }
    }

    public void SetHealth(float amt)
    {
        health = amt;
        HealthScript.UpdateHealth(health);
    }

    IEnumerator Kick(Damage dmg)
    {
        int kickTime = 40;
        Vector3 velocity = new Vector3();
        //velocity.y += 3f * dmg.kickMult;
        velocity.x = -Vector3.Normalize(dmg.source.transform.position - transform.position).x * 10f * dmg.kickMult;
        velocity.y = -Vector3.Normalize(dmg.source.transform.position - transform.position).y * 10f * dmg.kickMult;
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

    IEnumerator Die()
    {
        dead = true;
        Forest.Sound.PlayOneShot(Resources.Load<AudioClip>("Sounds/dieSound"));
        yield return StartCoroutine(LevelManager.Fogger.FogScreen());

        if (TotemScript.activatedTotems < 4)
            transform.position = GameObject.Find("RespawnPoint").transform.position;
        else
            transform.position = totemSpawn;



        dead = false;
        SetHealth(healthMax);

        BulletUI.SetBullets(10);
        bullets = 10;

        Forest.Sound.PlayOneShot(Resources.Load<AudioClip>("Sounds/fogFadeAway"));
        yield return StartCoroutine(LevelManager.Fogger.ShowScreen());

        if (TotemScript.activatedTotems < 4 && TotemScript.activatedTotems != 3)
            HelpText.Say("That hurt. Welp. I still have " + (4 - TotemScript.activatedTotems).ToString() + " totems to go.");
        else if (TotemScript.activatedTotems == 3)
            HelpText.Say("That hurt. Welp, just one totem left to go.");
        else
            HelpText.Say("Poop. I just need to get to the Enlightener, and it'll kill all of those monsters.");


        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fog" && collision.gameObject.GetComponent<FogScript>().sweeped == false)
        {
            if (!HelpText.AlreadySaid(HelpText.p_broom))
                HelpText.Say(HelpText.p_broom);
            nearbyFogs.Add(collision.gameObject);
        }

        if (collision.gameObject.tag == "Item")
            nearbyItems.Add(collision.gameObject);

        if (collision.gameObject.tag == "Activatable")
        {
            nearbyActives.Add(collision.gameObject);
            HelpText.SayIndefinitely("[E]");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fog")
            nearbyFogs.Remove(collision.gameObject);

        if (collision.gameObject.tag == "Item")
            nearbyItems.Remove(collision.gameObject);

        if (collision.gameObject.tag == "Activatable")
        {
            nearbyActives.Remove(collision.gameObject);
            HelpText.Shush();
        }
    }


}
