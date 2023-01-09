using UnityEngine;
using System.Collections;

public class MoveTrackNearestEnemy : MovementBase
{    
    public float TrackSpeed = 1;
    EnemyBase selectedEnemy;
    public override void Start () {
        base.Start();
        EnemyBase[] enemies = GameObject.FindObjectsOfType(typeof(EnemyBase)) as EnemyBase[];
        float prevDist = 10000;
       
        foreach(EnemyBase en in enemies)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(en.transform.position);            
            if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1) {
                float dist = Vector3.Distance(this.transform.position, en.transform.position);
                if (dist < prevDist && en.transform.position.x > GameObject.Find("Player").transform.position.x)
                {
                    selectedEnemy = en;
                    prevDist = dist;
                }
            }
        }



	}
	

    public override void Move()
    {
       if(selectedEnemy != null)
        {
            Vector3 toTarget = selectedEnemy.transform.position - transform.position;
            toTarget.z = transform.right.z;
            transform.right += toTarget*TrackSpeed*Time.deltaTime * ease;
        }
    
    }
}
