using UnityEngine;
using System.Collections;

public class CollisionEventDamage : CollisionEventBase
{

    public override void performCollideEvent(Collider2D otherObj)
    {


        bool isFriendly = gameObject.GetComponent<ProjectileBase>().isFriendly;



        if (isFriendly)
        {
            if (otherObj.gameObject.tag == "Enemy")
            {
                AffectEnemy(otherObj.gameObject);
            }
        }
        else
        {
            if (otherObj.gameObject.tag == "Player")
            {
                AffectPlayer(otherObj.gameObject);
            }
        }
    }


    void AffectEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyBase>().WhenHit(this.gameObject);
    }

    void AffectPlayer(GameObject player)
    {
        player.GetComponent<Player>().ChangeHealth(-this.GetComponent<ProjectileBase>().BaseDamage);
    }

}
