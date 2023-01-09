using UnityEngine;
using System.Collections;

public class ActionGrow : ActionBase
{

    public float GrowSpeed;
    public float MaxSize;
  


    protected override void performAction()
    {
        if (this.transform.localScale.x < MaxSize) {
            float amt = GrowSpeed * Time.deltaTime;
            this.transform.localScale = new Vector3(this.transform.localScale.x + amt, this.transform.localScale.y + amt, this.transform.localScale.z + amt);
                }


    }
}
