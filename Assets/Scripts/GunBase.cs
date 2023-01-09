using UnityEngine;
using System.Collections;

public class GunBase : MonoBehaviour {
    public float CooldownMax;
    public string ProjectileType = "ProjectileBase";
    public bool IsBeam = false;  
    float cooldown = 0;
    GameObject MyBeam;

    [FMODUnity.EventRef]
    public string shotSound = "event:/Gun";

	// Use this for initialization
	void Start () {
        if (IsBeam)
        {
            MyBeam = Instantiate(Resources.Load("Beams/" + ProjectileType)) as GameObject;
            MyBeam.GetComponent<BeamBase>().SetOrigin(transform.Find("BulletSpawner").transform);
            MyBeam.transform.position = transform.Find("BulletSpawner").transform.position;
            MyBeam.transform.parent = transform.Find("BulletSpawner").transform;
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (cooldown > 0)
            cooldown--;
	}

    public void Unfire()
    {
        if(IsBeam)
            MyBeam.GetComponent<BeamBase>().Go = false;
    }

    public void Fire(Vector3 dir)
    {
        if(cooldown <= 0 && !IsBeam)
        {

            FMODUnity.RuntimeManager.PlayOneShot(shotSound);
            cooldown = CooldownMax;


            GameObject to = Instantiate(Resources.Load("Projectiles/" + ProjectileType)) as GameObject;

            to.GetComponent<ProjectileBase>().Str = 1;
            to.transform.position = this.transform.Find("BulletSpawner").position;

            to.GetComponent<ProjectileBase>().Direction = Vector3.Normalize(dir - to.transform.position);
            

            dir.z = to.transform.position.z;

            to.transform.parent = GameObject.Find("Projectiles").transform;
            //ProjectileMoveBase PMB = to.GetComponent<ProjectileMoveBase>();

            // PMB.Speed = 20;



        }
        else if(IsBeam)
        {
            MyBeam.transform.rotation = transform.Find("BulletSpawner").transform.rotation;

            MyBeam.GetComponent<BeamBase>().Go = true;

        }

    }

}
