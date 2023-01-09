using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float maxSpeed;
    public float moveAcceleration;
    public float moveDeceleration;
    public float BaseHealth;
    float accMod = 0;
    float maxSpeedMod = 0;
    float currentHealth;
    public GameObject Arm;
    public GameObject Gun;
    public int Coins;
    public UIManager UI;
    float currentSpeed;
    string currentGun = "BaseGun";
    Profile profile;
    // Use this for initialization
    void Start () {
        currentHealth = BaseHealth;
        profile = GameObject.Find("Profile").GetComponent<Profile>();

        UI.ManualUpdate();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Time.timeScale > 0)
        {
            float vert = Input.GetAxis("Vertical");

            if (vert < 0)
            {
                vert = -1;
            }
            else if (vert > 0)
            {
                vert = 1;
            }
            else
            {
                currentSpeed *= moveDeceleration;
            }



            currentSpeed += moveAcceleration * vert;


            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;

            if (currentSpeed < -maxSpeed) currentSpeed = -maxSpeed;


            Vector3 moveAmount = new Vector3(this.transform.position.x, this.transform.position.y + currentSpeed * Time.deltaTime, this.transform.position.z);
            this.transform.position = moveAmount;



            Vector3 mousePosition;


            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);


            Vector3 dirToMouse = mousePosition - Arm.transform.position;


            float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;

            Arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Input.GetMouseButton(0))
            {
                Gun.GetComponent<GunBase>().Fire(mousePosition);
            }else if (!Input.GetMouseButton(0))
            {
                Gun.GetComponent<GunBase>().Unfire();
            }
        }
    }

    public void ChangeHealth(float amt)
    {
        currentHealth += amt;

        if (currentHealth <= 0) Die();

        UI.ManualUpdate();
    }
    public void AddCoins(int val)
    {
        Coins += val;
        UI.ManualUpdate();
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    void Die()
    {
        profile.ChangeCoins(this.Coins);
        profile.SaveProfile();
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ChangeGun(string gunType)
    {
        if (!gunType.Equals(currentGun))
        {
            currentGun = gunType;
            Transform gtransform = Gun.transform;
            Destroy(Gun);
            Gun = Instantiate(Resources.Load("Guns/" + gunType)) as GameObject;
            Gun.transform.parent = gtransform.parent;
            Gun.transform.localScale = gtransform.localScale;
            Gun.transform.localRotation = gtransform.localRotation;
            Gun.transform.position = Arm.transform.Find("GunSocket").position;
        }

    }

}
