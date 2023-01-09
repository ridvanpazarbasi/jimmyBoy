using UnityEngine;
using System.Collections;

public class ProjectileBase : MonoBehaviour {
    public float Lifespan =1;
    public float Str = 1;
    public float BaseSpeed = 1;
    public float BaseDamage = 1;
    public bool CanHurtOwner = false;
    public bool isFriendly = true;
    public bool LoopMovementStack = true;
    public bool LoopActionStack = true;
    public Vector3 Direction;
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


        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
	
	// Update is called once per frame
	void Update () {
        Lifespan -= 1 * Time.deltaTime;

        if (Lifespan < 0)
            Destroy(this.gameObject);

        DoMoveStack();
        DoActionStack();
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
}
