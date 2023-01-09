using UnityEngine;
using System.Collections;

public class MoveDrift : MovementBase  {
    public float MoveSpeed = 1;
    Vector3 Dir;
	// Use this for initialization
	public override void Start () {
        base.Start();
        Dir = new Vector3(Random.Range(1, 3)-1, Random.Range(0, 3)-1,0);
	}
	

    public override void Move()
    {
        Vector3 move = Dir * MoveSpeed * Time.deltaTime * ease;
        transform.position += move;
    }

}
