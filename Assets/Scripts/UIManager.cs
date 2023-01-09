using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour {
    public Player player;
    public Text Coins;
    public Text Health;
    public AudioMixer master;
    public Slider SFXSlider;
    public Slider MusicSlider;
    Profile profile;
    // Use this for initialization
    void Start () {
       profile = GameObject.Find("Profile").GetComponent <Profile>();

        SFXSlider.value = profile.SFXVol;
        MusicSlider.value = profile.MusicVol;
    }
	
	// Update is called once per frame
	void Update () {
        GameObject bo = transform.Find("Blackout").gameObject;
        Color oc = bo.GetComponent<Image>().color;
        bo.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Clamp(oc.a - Time.deltaTime*.5f,0,1));
    }

    public void ManualUpdate()
    {
        Coins.text = player.Coins.ToString();
        Health.text = player.GetCurrentHealth().ToString();
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
}
