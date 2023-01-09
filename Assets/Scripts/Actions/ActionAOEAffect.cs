using UnityEngine;
using System.Collections;

public class ActionAOEAffect : ActionBase
{


    public float Radius = 0;



    protected override void performAction()
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType(typeof(EnemyBase)) as EnemyBase[];

        bool isFriendly = gameObject.GetComponent<ProjectileBase>().isFriendly;
        bool canHurtOwner = gameObject.GetComponent<ProjectileBase>().CanHurtOwner;
        foreach (EnemyBase eb in enemies) { 
            if ((isFriendly || canHurtOwner) && Vector3.Distance(eb.gameObject.transform.position,this.transform.position) < Radius)
            {
                GameObject otherObj = eb.gameObject;
                if (otherObj.gameObject.tag == "Enemy")
                {
                    eb.WhenHit(this.gameObject);                   
                }
            }
         }





        float pDist = Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position);

        if(pDist < Radius)
            if (!isFriendly || canHurtOwner)
            {
                GameObject.Find("Player").GetComponent<Player>().ChangeHealth(-this.GetComponent<ProjectileBase>().BaseDamage);                                             
            }

    }
}
