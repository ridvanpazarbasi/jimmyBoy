using UnityEngine;
using System.Collections;


public class Profile : MonoBehaviour {
    public float SFXVol = 1;
    public float MusicVol = 1;
    public bool isLoaded = false;
    int coins = 0;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadProfile()
    {        
        SFXVol = PlayerPrefs.GetFloat("SFXVol");
        MusicVol = PlayerPrefs.GetFloat("MusicVol");
        coins = (int)PlayerPrefs.GetFloat("Coins");
        if (!isLoaded)
        {
            isLoaded = true;
        }
    }

    public void SaveProfile()
    {
        PlayerPrefs.SetFloat("SFXVol", SFXVol);
        PlayerPrefs.SetFloat("MusicVol", MusicVol);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.Save();
    }


    public void Exit()
    {
        SaveProfile();
        Application.Quit(); 
    }

    public void ChangeCoins(int val)
    {
        coins += val;
        if (coins < 0) coins = 0;
    }

    public int GetCoins()
    {
        return coins;
    }
}
