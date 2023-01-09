using UnityEngine;
using System.Collections;

public class DropBase : MonoBehaviour {
    public float HowCommon = 100;
    public float Lifespan = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0) Destroy(gameObject);
	   
	}
}
