using UnityEngine;
using System.Collections;

public class DropCollideGiveGun : DropCollideBase {

    public string GunType;
 
    public override void AffectPlayer(GameObject otherObj)
    {
        otherObj.GetComponent<Player>().ChangeGun(GunType);
    }


}

