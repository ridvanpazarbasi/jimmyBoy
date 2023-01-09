using UnityEngine;
using System.Collections;

public class MoveForward : MovementBase
{
    public float MoveSpeed = 1;
    public override void Start () {
        base.Start();
        
	}
	

    public override void Move()
    {
       
        transform.position += transform.right * MoveSpeed * Time.deltaTime * ease;
    }
}
