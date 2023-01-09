using UnityEngine;
using System.Collections;

public class ProjectileCollideBase : MonoBehaviour
{

    CollisionEventBase[] collisionEvents;
    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.gameObject.tag == "Player" || otherObj.gameObject.tag == "Wall" || otherObj.gameObject.tag == "Enemy") { 
            collisionEvents = GetComponents<CollisionEventBase>();

            foreach (CollisionEventBase ce in collisionEvents)
            {
                ce.performCollideEvent(otherObj);
            }
        }
    }
}
