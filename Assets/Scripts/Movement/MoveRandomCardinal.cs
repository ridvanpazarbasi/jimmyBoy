using UnityEngine;
using System.Collections;

public class MoveRandomCardinal : MovementBase  {
    public float MoveSpeed = 1;
    public float PushRightMultiplier = 2;
    float moveMult = 0;
    float dir;
    Quaternion startDir;
    // Use this for initialization
    public override void Start () {
        base.Start();
        startDir = transform.rotation;
        currentDuration -= Time.deltaTime;
        dir = 0;
        moveMult = 1;
    }
	

    public override void Move()
    {
         if (currentDuration == Duration)
         {
           
         }
        
        Vector3 move = transform.right * MoveSpeed * Time.deltaTime * ease * moveMult;

        transform.position += move;
    }

    void GetRandomFacing()
    {
        float ndir = Mathf.Floor(Random.Range(0, 2));
        if (dir == 0 || dir == 2)
        {
            if (ndir == 0)
                dir = 1;
            else
                dir = 3;
        }
        else if(dir == 1 || dir == 3)
        {
            if (ndir == 0)
                dir = 0;
            else
                dir = 2;
        }
        else
        {
            dir = 0;
        }
           
        if (Random.Range(0,2) < 1) dir = 0;
    }



    public override void Reset()
    {
        GetRandomFacing();
        transform.rotation = startDir;
        moveMult = 1;
        float directionMod = dir * 90;
        if (dir == 0)
        {

            moveMult = PushRightMultiplier;
        }
        else
            transform.Rotate(0, 0, directionMod);

        base.Reset();
    }
}
