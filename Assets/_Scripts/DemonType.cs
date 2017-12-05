using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonType {

    public enum Type
    {
        Null,
        Dresh,
        Kirgan
    }

    public float senseDistance;
    public float speed = 2;
    public float attackDist = 2;

    public float maxHealth = 100;
    public float health;

    public string name;

    public float damage;
    public float hitDistance;

    GameObject target;
    DemonScript demon;

    public AudioClip walkSound;
    public AudioClip dieSound;
    public AudioClip alertSound;
    public AudioClip hurtSound;

    Sprite sprite;

    public static List<DemonType> types = new List<DemonType>();

    public virtual void Init(DemonScript demon)
    {
        name = "Unknown Demon";
        this.demon = demon;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    public virtual void Attack()
    {

    }

    public virtual void Run()
    {

    }



    public class Dresh : DemonType
    {
        int chargeTick = 10;
        int chargeTickMax = 10;

        int jumpTick = 30;
        int jumpTickMax = 30;

        int runTick = 35;
        int runTickMax = 35;

        Sprite sprite;

        public override void Init(DemonScript demon)
        {
            speed = 5;
            attackDist = 3;
            name = "Dresh";
            this.demon = demon;
            maxHealth = 60;
            health = maxHealth;

            hitDistance = 0.2f;
            damage = 15;

            walkSound = Resources.Load<AudioClip>("Sounds/demonWalk");
            dieSound = Resources.Load<AudioClip>("Sounds/dreshDie");
            alertSound = Resources.Load<AudioClip>("Sounds/dreshAlert");
            hurtSound = Resources.Load<AudioClip>("Sounds/dreshHurt");

            sprite = Resources.Load<Sprite>("Demons/dreshWalk");
        }

        public override void Attack()
        {
            // playsound
            //set sprite

            if (demon.curState == DemonScript.State.Chasing)
            {
                if (demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled == false)
                    demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled = true;
                if (chargeTick > 0)
                {
                    demon.PlaySound(Resources.Load<AudioClip>("Sounds/demonCharge"));
                    demon.velocity = new Vector3();
                    chargeTick--;
                }
                else // jump
                {                    
                    demon.PlaySound(Resources.Load<AudioClip>("Sounds/demonAttack1"));
                    demon.velocity = Vector3.Normalize(target.transform.position - demon.transform.position) * speed * 5f;
                    demon.curState = DemonScript.State.Jumping;
                }
            } else if  (demon.curState == DemonScript.State.Jumping)
            {
                if (jumpTick > 0)
                {
                    jumpTick--;
                } else
                {
                    demon.velocity = new Vector3();
                    Run();
                }
            }

        }

        public override void Run()
        {
            if (demon.curState != DemonScript.State.Running && demon.curState != DemonScript.State.Prowling)
            {
                demon.curState = DemonScript.State.Running;
                chargeTick = chargeTickMax;
                jumpTick = jumpTickMax;

                demon.velocity = Vector3.Normalize(demon.transform.position - target.transform.position) * speed * 2f;
                if (demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled == true)
                    demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled = false;

            } else
            {
                if (demon.curState == DemonScript.State.Running)
                {
                    runTick--;
                    if (runTick <= 0)
                    {
                        demon.Wander(true);
                        demon.curState = DemonScript.State.Prowling;
                        runTick = runTickMax * 3;
                    }
                }
                if (demon.curState == DemonScript.State.Prowling)
                {
                    runTick--;
                    if (runTick <= 0)
                    {
                        demon.curState = DemonScript.State.Chasing;
                        runTick = runTickMax;
                    }
                }
                


            }
        }




    }

    public class Kirgan : DemonType
    {
        int chargeTick = 5;
        int chargeTickMax = 5;

        int jumpTick = 20;
        int jumpTickMax = 20;

        int runTick = 20;
        int runTickMax = 20;
        
        Sprite sprite;

        public override void Init(DemonScript demon)
        {
            speed = 3;
            attackDist = 5;
            name = "Kirgan";
            this.demon = demon;
            maxHealth = 250;
            health = maxHealth;

            hitDistance = 1f;


            damage = 30;

            walkSound = Resources.Load<AudioClip>("Sounds/kirganStep2");
            alertSound = Resources.Load<AudioClip>("Sounds/kirganAlert");
            dieSound = Resources.Load<AudioClip>("Sounds/kirgan6");
            hurtSound = Resources.Load<AudioClip>("Sounds/kirganHurt");

            sprite = Resources.Load<Sprite>("Demons/kirgan");
        }

        public override void Attack()
        {
            // playsound
            //set sprite

            if (demon.curState == DemonScript.State.Chasing)
            {
                if (demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled == false)
                    demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled = true;
                if (chargeTick > 0)
                {
                    demon.PlaySound(Resources.Load<AudioClip>("Sounds/kirgan1"));
                    demon.velocity = new Vector3();
                    chargeTick--;
                }
                else // jump
                {
                    demon.PlaySound(Resources.Load<AudioClip>("Sounds/kirgan4"));
                    demon.velocity = Vector3.Normalize(target.transform.position - demon.transform.position) * speed * 5f;
                    demon.curState = DemonScript.State.Jumping;
                }
            }
            else if (demon.curState == DemonScript.State.Jumping)
            {
                if (jumpTick > 0)
                {
                    jumpTick--;
                }
                else
                {
                    demon.velocity = new Vector3();
                    Run();
                }
            }

        }

        public override void Run()
        {
            if (demon.curState != DemonScript.State.Running && demon.curState != DemonScript.State.Prowling)
            {
                demon.curState = DemonScript.State.Running;
                chargeTick = chargeTickMax;
                jumpTick = jumpTickMax;

                demon.velocity = Vector3.Normalize(demon.transform.position - target.transform.position) * speed * 2f;
                if (demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled == true)
                    demon.transform.Find("Sprite").GetComponent<LookTowards>().enabled = false;

            }
            else
            {
                if (demon.curState == DemonScript.State.Running)
                {
                    runTick--;
                    if (runTick <= 0)
                    {
                        demon.Wander(true);
                        demon.curState = DemonScript.State.Prowling;
                        runTick = runTickMax * 3;
                    }
                }
                if (demon.curState == DemonScript.State.Prowling)
                {
                    runTick--;
                    if (runTick <= 0)
                    {
                        demon.curState = DemonScript.State.Chasing;
                        runTick = runTickMax;
                    }
                }



            }
        }




    }

}
