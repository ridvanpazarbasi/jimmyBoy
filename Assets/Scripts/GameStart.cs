using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameStart : MonoBehaviour {
    public Profile profile;
    public GameObject Fader;
    public AudioMixer master;
    public Slider SFXSlider;
    public Slider MusicSlider;
    public ProfileManager profileManager;
    bool FadeIn = true;
    // Use this for initialization
    void Start()
    {
        
        if (GameObject.Find("Profile") != null)
        {
           
           
            transform.Find("Main").gameObject.SetActive(false);
            transform.Find("Profile").gameObject.SetActive(true);
            profile = GameObject.Find("Profile").GetComponent<Profile>();

        }
        else
        {
            GameObject pro = Instantiate(Resources.Load("Profile")) as GameObject;
            pro.name = "Profile";
            
            profile = pro.GetComponent<Profile>();
            profile.LoadProfile();
        }




        profileManager.Setup();

        master.SetFloat("SFXVol", profile.SFXVol);        
        master.SetFloat("MusicVol", profile.MusicVol);
        SFXSlider.value = profile.SFXVol;
        MusicSlider.value = profile.MusicVol;


    }
	
	// Update is called once per frame
	void Update () {
        if (FadeIn)
        {
            Color c = Fader.GetComponent<Image>().color;
            if (c.a > 0) c.a -= Time.deltaTime;
            Fader.GetComponent<Image>().color = c;
        }
        else
        {
            Color c = Fader.GetComponent<Image>().color;
            if (c.a <= 1)
                c.a += 10* Time.deltaTime;
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            Fader.GetComponent<Image>().color = c;
        }
	}

    
    public void StartGame()
    {
        FadeIn = false;
        

    }

    public void SetSFXVol(float val)
    {
        profile.SFXVol = val;
        master.SetFloat("SFXVol", val);
    }

    public void SetMusicVol(float val)
    {
        profile.MusicVol = val;
        master.SetFloat("MusicVol", val);
    }

    public void SaveProfile()
    {
        profile.SaveProfile();
    }
}
