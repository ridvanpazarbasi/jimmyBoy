using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {
    public Text StatusCoins;
    Profile profile;
	// Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Setup()
    {
        profile = GameObject.Find("Profile").GetComponent<Profile>();
        StatusCoins.text = "" + profile.GetCoins();
    }
}
