using UnityEngine;
using System.Collections;

public class CollisionEventDie : CollisionEventBase
{

    public override void performCollideEvent(Collider2D otherObj)
    {


        bool isFriendly = gameObject.GetComponent<ProjectileBase>().isFriendly;

        if (otherObj.gameObject.tag == "Wall" && GetComponent<ProjectileBase>().isFriendly || otherObj.gameObject.tag == "BlockEverything")
        {
            Die(otherObj.gameObject);
            return;
        }


        if (isFriendly)
        {
            if (otherObj.gameObject.tag == "Enemy")
            {               
                Die(otherObj.gameObject);
            }
        }
        else
        {
            if (otherObj.gameObject.tag == "Player")
            {
             
                Die(otherObj.gameObject);
            }
        }
    }

    void Die(GameObject otherObj)
    {
        Destroy(gameObject);
    }


}
