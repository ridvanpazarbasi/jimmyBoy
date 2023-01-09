using UnityEngine;
using System.Collections;

public class DropCollideBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D otherObj)
    {
       

        if (otherObj.gameObject.tag == "Player")
        {
            AffectPlayer(otherObj.gameObject);
            Die();
            return;
        }
    }
 
    public virtual void AffectPlayer(GameObject otherObj)
    {

    }

    void Die()
    {
        //otherObj.GetComponent<Player>().ChangeGun("GunBeam");
        Destroy(gameObject);
    }

}

