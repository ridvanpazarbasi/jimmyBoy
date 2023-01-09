using UnityEngine;
using System.Collections;

public class BeamCollideBase : MonoBehaviour {


    public void OnBeamHit(Collider2D otherObj)
    {
        bool isFriendly = gameObject.GetComponent<BeamBase>().isFriendly;
        
        if (otherObj.gameObject.tag == "Wall" && GetComponent<BeamBase>().isFriendly)
        {
           // Die(otherObj.gameObject);
            return;
        }

        
        if (isFriendly)
        {
            if (otherObj.gameObject.tag == "Enemy")
            {
                AffectEnemy(otherObj.gameObject);
                //Die(otherObj.gameObject);
            }
        }
        else
        {
            if (otherObj.gameObject.tag == "Player")
            {
                AffectPlayer(otherObj.gameObject);
                //Die(otherObj.gameObject);
            }
        }
    }

    void Die(GameObject otherObj)
    {
        Destroy(gameObject);
    }

    void AffectEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyBase>().WhenHit(this.gameObject);
    }

    void AffectPlayer(GameObject player)
    {
        player.GetComponent<Player>().ChangeHealth(-this.GetComponent<BeamBase>().BaseDamage);
    }

}
