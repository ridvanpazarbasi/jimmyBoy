using UnityEngine;
using System.Collections;

public class DropCollideGiveCoin : DropCollideBase {

    public int Amount = 1;
 
    public override void AffectPlayer(GameObject otherObj)
    {
        otherObj.GetComponent<Player>().AddCoins(Amount);
    }


}

