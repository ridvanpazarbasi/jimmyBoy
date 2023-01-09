using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {
    public float PointCost = 5;
    public string Tileset = "Any";
    public float Health = 1;
    public float Str = 1;
    public float Speed = 1;
    public float Lifespan = 10;
    public bool MoveWithWorld = true;
    public bool LoopMovementStack = true;
    public bool LoopActionStack = true;   
    MovementBase[] movements;
    ActionBase[] actions;
    // Use this for initialization
    void Start () {
        movements = GetComponents<MovementBase>();
        for (int x = 0; x < movements.Length; x++)
        {
            movements[x].Start();
        }


        actions = GetComponents<ActionBase>();

        if (!MoveWithWorld)
        {
            transform.parent = transform.parent.parent.parent;
        }
    }
    public void UpdateFromStats()
    {
        float s = 1 * (1 + (Health * .03f));
        transform.localScale = new Vector3(s,s,s);
        this.transform.Find("GFX").gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1-Str*.01f, 1 - Speed * .01f,1);
    }
    // Update is called once per frame
    void Update() {
        UpdateFromStats();

        Lifespan -= 1 * Time.deltaTime;

        if (Lifespan <= 0) Destroy(gameObject);

        DoMoveStack();
        if(transform.position.x-3 > GameObject.Find("Player").transform.position.x) DoActionStack();
	}

    void DoActionStack()
    {
        int inactiveCount = 0;
        for (int x = 0; x < actions.Length; x++)
        {
            if (actions[x].DoAction)
            {
                if (x == 0)
                    actions[x].CheckAction();

                else if (actions[x].WaitForPrevious && actions[x - 1].DoAction)
                    x = actions.Length;
                else
                {
                    actions[x].CheckAction();
                }


            }
            else
                inactiveCount++;

        }


        if (inactiveCount == actions.Length && LoopActionStack)
        {

            for (int x = 0; x < actions.Length; x++)
            {
                actions[x].Reset();
            }
        }
    }
    void DoMoveStack()
    {
        int inactiveCount = 0;
        for (int x = 0; x < movements.Length; x++)
        {
            if (movements[x].DoMove)
            {
                if (x == 0)
                    movements[x].CheckMove();

                else if (movements[x].WaitForPrevious && movements[x - 1].DoMove)
                    x = movements.Length;
                else
                {
                    movements[x].CheckMove();
                }


            }
            else
                inactiveCount++;

        }


        if (inactiveCount == movements.Length && LoopMovementStack)
        {

            for (int x = 0; x < movements.Length; x++)
            {
                movements[x].Reset();
            }
        }
    }
    public void WhenHit(GameObject HitBy)
    {
        if (HitBy.GetComponent<ProjectileBase>())
        {
            Health -= HitBy.GetComponent<ProjectileBase>().BaseDamage;
            if (Health <= 0) Die(HitBy);
        }
        else if (HitBy.GetComponent<BeamBase>())
        {
            Health -= HitBy.GetComponent<BeamBase>().BaseDamage * Time.deltaTime;
            if (Health <= 0) Die(HitBy);
        }
    }

    void Die(GameObject HitBy)
    {

        SpawnDrop();
        Destroy(gameObject);
    }

    void SpawnDrop()
    {
        GameObject.Find("World").GetComponent<WorldManager>().GenerateDrop(this.transform);

    }
}
