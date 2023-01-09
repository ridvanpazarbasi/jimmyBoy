using UnityEngine;
using UnityEngine.Advertisements;

public class AdControl : MonoBehaviour
{



#if UNITY_PRO_LICENSE
    //Place Script on a empty gamobject in Start Menu

    static bool GameStarted;

    public void Start()
    {
        if (GameStarted)
        {
            ShowAd();
        }

        if (!GameStarted)
        {
            GameStarted = true;
        }
    }

    public void ShowAd()
    {
      
    }

#endif

}

public class Advertisement

{
    

    public static void Show()
    {
        throw new System.NotImplementedException();
    }
}
