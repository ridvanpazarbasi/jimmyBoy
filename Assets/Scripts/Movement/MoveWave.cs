using UnityEngine;
using System.Collections;

public class MoveWave : MovementBase {    
    public float Frequency = 20.0f;
    public float Magnitute = .5f;    
    public bool DoY = true;
    public bool DoX = false;
    public float StartPoint = -1;
    float myTime = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (StartPoint == -1)
            StartPoint = Random.Range(0, 100);
    }


    public override void Move()
    {
        myTime += Time.deltaTime;
        if(DoY)transform.position = transform.position + transform.up * Mathf.Sin((myTime + StartPoint) * Frequency) * Magnitute * Time.deltaTime * ease;
        if(DoX) transform.position = transform.position + transform.right * Mathf.Cos((myTime + StartPoint ) * Frequency) * Magnitute * Time.deltaTime * ease;

    }
}
