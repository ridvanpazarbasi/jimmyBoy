using UnityEngine;
using System.Collections;

public class CollisionEventFireBullet : CollisionEventBase
{

   

    public string BulletType;
    public Vector3 OriginRelative;
    public Transform Origin;

    public float Inaccuracy = 0;
    public float AngleChange = 0;
    public bool UseOwnerDirection = false;
    public bool LockToOwner = false;


    public override void performCollideEvent(Collider2D otherObj)
    {
        Vector3 origin;
        if (Origin)
        {
            origin = Origin.position;
        }
        else
        {
            origin = transform.position + OriginRelative;
        }




        GameObject to = Instantiate(Resources.Load("Projectiles/" + BulletType)) as GameObject;
        to.transform.position = origin;
        to.GetComponent<ProjectileBase>().isFriendly = true;
        if (GetComponent<ProjectileBase>())
            to.GetComponent<ProjectileBase>().isFriendly = this.GetComponent<ProjectileBase>().isFriendly;
        else if (GetComponent<BeamBase>())
            to.GetComponent<ProjectileBase>().isFriendly = this.GetComponent<BeamBase>().isFriendly;
        else if (GetComponent<EnemyBase>())
            to.GetComponent<ProjectileBase>().isFriendly = false;
        if (LockToOwner)
        {
            to.transform.SetParent(transform);
            to.GetComponent<ProjectileBase>().Direction = transform.right;
        }
        else if (UseOwnerDirection == false)
        {
            Vector3 nDir = Vector3.Normalize(GameObject.Find("Player").transform.position - to.transform.position);


            float angle = Mathf.Atan2(nDir.y, nDir.x) * Mathf.Rad2Deg;

            angle += Random.Range(-Inaccuracy, Inaccuracy);

            to.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            to.GetComponent<ProjectileBase>().Direction = to.transform.right;

        }
        else {


            float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

            angle += Random.Range(-Inaccuracy, Inaccuracy) + AngleChange;

            to.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            to.GetComponent<ProjectileBase>().Direction = to.transform.right;


        }
        if (!LockToOwner)
            to.transform.parent = GameObject.Find("Projectiles").transform;
        //ProjectileMoveBase PMB = to.GetComponent<ProjectileMoveBase>();

        //PMB.Speed = 5;


    }
}