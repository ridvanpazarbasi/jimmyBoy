using UnityEngine;
using System.Collections;

public class MovementBase : MonoBehaviour
{
    protected float Duration = 1;
    public float DurationMin = 1;
    public float DurationMax = 1;
    protected float currentDuration;
    public bool WaitForPrevious = true;
    public bool DoEaseOff = true;
    public bool DoEaseIn = true;
    public float EaseSpeed = .5f;
    public bool DoMove = true;

    protected float ease = 1;
    // Use this for initialization
    public virtual void Start () {
        if (DoEaseIn) ease = 0;
        Duration = Random.Range(DurationMin, DurationMax);
        currentDuration = Duration;
	}
	
    public virtual void Move()
    {
        
    }
	// Update is called once per frame
	public void Update () {        

    }

    public void CheckMove()
    {
        
        if (currentDuration <= 0)
        {
            DoMove = false;
        }

        if (DoEaseOff && currentDuration < (Duration * .5f*EaseSpeed) && DoMove)
        {
            ease = (currentDuration / (Duration * .5f * EaseSpeed));

        }

        if (DoEaseIn && currentDuration > Duration - (Duration * .5f * EaseSpeed) && DoMove)
        {
            ease = 1 - (currentDuration % (Duration * .5f * EaseSpeed) / (Duration * .5f * EaseSpeed));
        }


        if (DoMove) Move();
        currentDuration -= Time.deltaTime;
    }
    public virtual void Reset()
    {
        if (DoEaseIn)
            ease = 0;
        else
            ease = 1;

        currentDuration = Duration;
        DoMove = true;
    }
}
