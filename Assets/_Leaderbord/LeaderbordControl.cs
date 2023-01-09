using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

/// <summary>
/// Place on an empty GameObject in MainMenu and actual Game
/// </summary>
public class LeaderbordControl : MonoBehaviour
{
    #region Variables
    #region Menu Display Variables

    public Text rankText;

    public Text topScore;

    public Image bageDisplay;
    public List<Sprite> bages = new List<Sprite>();

    public List<Image> stars = new List<Image>();

    public GameObject store;

    GameObject leaderbordInfo;

    #endregion

    string leaderboardID = "CgkIm6r__KYWEAIQAQ";

    public static int score = 0;

    public static float time = 0;

    public bool inGame;

    #region InGame Display Variables

    public Text scoreDisplay;

    #endregion

    #endregion

    #region MonoBehaviour Functions

    void Start()
    {
        PlayGamesPlatform.Activate();

        if (!Social.localUser.authenticated)
        {
            SignIn();
        }

        foreach (Transform i in transform)
        {
            if (i.name == "LeaderboardInfo")
            {
                leaderbordInfo = i.gameObject;
            }
        }

        if(!inGame)
        {
            EndGame();
        }
    }

    void Update()
    {
        if (inGame)
        {
            time += Time.deltaTime;

            scoreDisplay.text = "Score : " + (score + (int)time);
        }
    }

    #endregion

    #region StatRecording functions

    public void AddScore(float adj)
    {
        score += (int)adj;
    }

    public void AddScore(int adj)
    {
        score += adj;
    }

    public void ReportToLeaderboard()
    {
        Social.ReportScore(score + (int)time, leaderboardID, (bool success) =>
        {
            if (success)
            {
                UpdateInfo();
            }
            else
            {
                SignIn(true, score + (int)time);
            }
        });
    }

    public void ShowWarning()
    {

    }

    public void CloseWarningWindow()
    {

    }

    public void EndGame()
    {
        ReportToLeaderboard();

        score = 0;

        time = 0;
    }

    void checkIfFirstTimePlaying()
    {
    //    if (PlayerPrefs.GetInt("FirstPlayed", 0) == 0)
    //    {

    //        PlayGamesPlatform.Instance.LoadScores(

    //   leaderboardID,
    //   LeaderboardStart.PlayerCentered,
    //   1,
    //   LeaderboardCollection.Public,
    //   LeaderboardTimeSpan.AllTime,
    //(LeaderboardScoreData data) =>
    //{
    //    if (!data.Valid)
    //    {
    //        ReportToLeaderboard();
    //        PlayerPrefs.SetInt("FirstPlayed", 1);
    //    }
    //    //Debug.Log(data.Valid);
    //    //Debug.Log(data.Id);
    //    //Debug.Log(data.PlayerScore);
    //    //data.PlayerScore.rank;
    //    //Debug.Log(data.PlayerScore.userID);
    //    //Debug.Log(data.PlayerScore.formattedValue);
    //});

    //        ShowNewGlobalRanking();
    //    }
    }

    #endregion

    #region UIFunctions

    public void ShowScoreInfo()
    {
        store.SetActive(false);
        leaderbordInfo.SetActive(true);
        UpdateInfo();
    }

    public void HideScoreInfo()
    {
        store.SetActive(true);
        leaderbordInfo.SetActive(false);
    }

    public void UpdateInfo()
    {
        PlayGamesPlatform.Instance.LoadScores(

    leaderboardID,
    LeaderboardStart.PlayerCentered,
    1,
    LeaderboardCollection.Public,
    LeaderboardTimeSpan.AllTime,
    (LeaderboardScoreData data) =>
    {
        checkIfFirstTimePlaying();
        if (rankText && data.Valid)
            rankText.text = "Rank : " + data.PlayerScore.rank.ToString();
        if (topScore && data.Valid)
            topScore.text = data.PlayerScore.value.ToString();

        int playerRank = -1;

        if (data.Valid)
        {
            playerRank = data.PlayerScore.rank;
        }

        #region bageDisplay selector
        if (playerRank != -1)
        {
            if (playerRank == 1)
            {

                bageDisplay.sprite = bages[0];

            }
            else if (playerRank >= 10)
            {
                bageDisplay.sprite = bages[1];
            }
            else if (playerRank >= 100)
            {
                bageDisplay.sprite = bages[2];
            }
            else if (playerRank >= 1000)
            {
                bageDisplay.sprite = bages[3];
            }
            else if (playerRank >= 2000)
            {
                bageDisplay.sprite = bages[4];
            }
            else if (playerRank >= 3000)
            {
                bageDisplay.sprite = bages[5];
            }
            else if (playerRank >= 4000)
            {
                bageDisplay.sprite = bages[6];
            }
            else if (playerRank >= 5000)
            {
                bageDisplay.sprite = bages[7];
            }
            else if (playerRank >= 7000)
            {
                bageDisplay.sprite = bages[8];
            }
            else if (playerRank >= 9000)
            {
                bageDisplay.sprite = bages[9];
            }
            else if (playerRank >= 10000)
            {
                bageDisplay.sprite = bages[10];
            }
            else if (playerRank >= 20000)
            {
                bageDisplay.sprite = bages[11];
            } 
        }
        #endregion

        //Debug.Log(data.PlayerScore.userID);
        //Debug.Log(data.PlayerScore.formattedValue);
    });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    void SignIn(bool reporteToLeaderboard = false, float NewMaxScore = 0)
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                checkIfFirstTimePlaying();

                if (reporteToLeaderboard)
                    ReportToLeaderboard();
            }
            else
            {
                ShowWarning();
            }
        });
    }

    public void _SignIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                checkIfFirstTimePlaying();
            }
            else
            {
                ShowWarning();
            }
        });
    }

    public void ShowLeaderBored()
    {
        //Social.ShowLeaderboardUI();
        PlayGamesPlatform.Activate();
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardID);
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
    }
    #endregion

    #region Null Functions
    public void ShowNewGlobalRanking()
    {
        PlayGamesPlatform.Instance.LoadScores(

           leaderboardID,
           LeaderboardStart.PlayerCentered,
           1,
           LeaderboardCollection.Public,
           LeaderboardTimeSpan.AllTime,
       (LeaderboardScoreData data) =>
       {
           //Debug.Log(data.Valid);
           //Debug.Log(data.Id);
           //Debug.Log(data.PlayerScore);
           //data.PlayerScore.rank;
           //Debug.Log(data.PlayerScore.userID);
           //Debug.Log(data.PlayerScore.formattedValue);
       });
    }
    #endregion
}

