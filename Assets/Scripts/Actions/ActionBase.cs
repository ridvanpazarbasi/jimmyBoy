using UnityEngine;
using System.Collections;

public class ActionBase : MonoBehaviour {
    public float WaitMax;
    public float WaitMin;
    public bool WaitForPrevious = true;
    public bool DoAction = true;
    protected float currentWait;
    // Use this for initialization
    void Start()
    {
        currentWait = Random.Range(WaitMin, WaitMax);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void CheckAction()
    {
        currentWait -= 1 * Time.deltaTime;
        if (currentWait <= 0)
        {
            DoAction = false;
            performAction();
            currentWait = WaitMax;
        }

    }
    protected virtual void performAction()
    {

    }

    public void Reset()
    {
        currentWait = Random.Range(WaitMin, WaitMax);
        DoAction = true;
    }
}
