using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;
using System.Collections.Generic;
using PlayFab.Json;
using UnityEngine.SceneManagement;




public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;

    private string userEmail;
    private string userPassword;
    private string username;
    private string myID;
    private GameObject loginPanel;

    public PlayerStats stats;

	// singleton
    private void OnEnable()
    {
        if (PFC == null)
        {
            PFC = this;
        }
        else
        {
            if (PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
			PlayFabSettings.TitleId = "7FE33"; // Please change this value to your own titleId from PlayFab Game Manager
        }

       
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        } else
     {
/*		 this is mobile login if you want the player just play and his data will be stored into like virtual account connected to his device   	
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnMobileLoginSuccess, OnMobileLoginFailure);
#endif

#if UNITY_IOS
            var requestIos = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIos, OnMobileLoginSuccess, OnMobileLoginFailure);
#endif  
*/
        }

    }

    public void GetUserEmail(string emailin)
    {
        userEmail = emailin;
    }

    public void GetUserPassword(string passwordin)
    {
        userPassword = passwordin;
    }

    public void GetUsername(string usernamein)
    {
        username = usernamein;
    }


 /*  
   #region Mobile Login
    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    private void OnMobileLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log("Mobile Login seccess!!");

        // loginPanel.SetActive(false);
        myID = result.PlayFabId;
        GetStats();
        GetPlayerData();
    }

    private void OnMobileLoginFailure(PlayFabError error)
    {
        Debug.Log("Mobile Login Failed!!");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion
*/
    #region PlayFab Login

    public void OnCLickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log("Login seccess!!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        myID = result.PlayFabId;
        GetStats();
        GetPlayerData();


		PlayerProfile.PP.LogoutButton.SetActive (true);
		PlayerProfile.PP.LoginButton.SetActive (false);
		PlayerProfile.PP.RegisterButton.SetActive (false);


		PlayerProfile.PP.loginRegistrationPanel.SetActive (false);
    }

    private void OnLoginFailure(PlayFabError error)
    {     
        Debug.Log("Login Failed!!");
    }

    #endregion
    

    public void OnClickLogOut()
    {
        PlayerPrefs.DeleteAll();
        LobbyManager.LM.StartMatchMaker();
        LobbyManager.LM.StopMatchMaker();
		SceneManager.LoadScene (0);
        Debug.Log("Log Out");

		PlayerProfile.PP.LoginButton.SetActive (true);
		PlayerProfile.PP.RegisterButton.SetActive (true);
    }


    #region PlayFab Registrasion

    public void OnClickRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username, DisplayName = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSccess, OnRegisterFailure);
    }

    private void OnRegisterSccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log("Registeration seccess!!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        // UpdateDisplayName();
        GameObject registration = GameObject.Find("Registration");
        registration.SetActive(false);

        OnCLickLogin();
    }


    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log("Registration Failed!!");
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region Update Username 

    public void UpdateDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = stats.username }, OnUpdateDisplayNameSuccess, OnLoginFailure);
    }

    void OnUpdateDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + " is your display name");
        stats.username = result.DisplayName;
    }

    public void GetDisplayName(string name)
    {
        stats.username = name;
    }

    public void GetPlayerProfile(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }

        },
        result => stats.username = result.PlayerProfile.DisplayName,
        error => Debug.LogError(error.GenerateErrorReport()));
		Debug.Log (stats.username);
    }



    public void GetPlayerProfile()
    {
        var request = new GetPlayerProfileRequest();
        // Request an API übergeben
        PlayFabClientAPI.GetPlayerProfile(request, OnGetplayerProfileSuccess, OnLoginFailure);
    }

    private void OnGetplayerProfileSuccess(GetPlayerProfileResult result)
    {
        stats.username = result.PlayerProfile.DisplayName;
    }
    #endregion

	/* recovering is  replacing the mobile login with the email login
    #region PlayFab Recovring

    public void OnClickRecover()
    {
        var registerRecover = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = stats.username };
        PlayFabClientAPI.AddUsernamePassword(registerRecover, OnRecoverSccess, OnRegisterFailure);
    }

    private void OnRecoverSccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log("Registeration seccess!!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

#if UNITY_ANDROID
      UnlinkAndroid();
#endif

#if UNITY_IOS
      UnlinkIos();    
#endif
        GameObject registration = GameObject.Find("Registration");
        registration.SetActive(false);
        loginPanel = GameObject.Find("Login/Register Panel");
        loginPanel.SetActive(false);
    }

#endregion
*/

	/*  unlink the device after recovering
#region Unlink

    private void UnlinkAndroid()
    {
        var unlinkAndroidRequest = new UnlinkAndroidDeviceIDRequest { };
        PlayFabClientAPI.UnlinkAndroidDeviceID(unlinkAndroidRequest,OnUnlinkAndroidSuccess,OnUnlinkFailure);
    }

    private void UnlinkIos()
    {
        var unlinkIosRequest = new UnlinkIOSDeviceIDRequest { };
        PlayFabClientAPI.UnlinkIOSDeviceID(unlinkIosRequest, OnUnlinkIosSuccess, OnUnlinkFailure); 
    }


    private void OnUnlinkAndroidSuccess(UnlinkAndroidDeviceIDResult result)
    {
        Debug.Log("Unlink Android Success !!");
    }

    private void OnUnlinkIosSuccess(UnlinkIOSDeviceIDResult result)
    {
        Debug.Log("Unlink IOS Success !!");
    }


    private void OnUnlinkFailure(PlayFabError error)
    {
        Debug.Log("Unlink Failed!!");
        Debug.LogError(error.GenerateErrorReport());
    }
#endregion
*/

#region Player Stats

    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Weapon", Value = stats.weapon },
        new StatisticUpdate { StatisticName = "Level", Value = stats.level },
        new StatisticUpdate { StatisticName = "Stars", Value = stats.stars },
        new StatisticUpdate { StatisticName = "Coins", Value = stats.coins },
        new StatisticUpdate { StatisticName = "Wins", Value = stats.wins},
        new StatisticUpdate { StatisticName = "Loss", Value = stats.loss },
        new StatisticUpdate { StatisticName = "Haki Level", Value = stats.hakilvl },
        new StatisticUpdate { StatisticName = "Haki Pills", Value = stats.hakipills },

            }
        },
  result => { Debug.Log("User statistics updated"); },
  error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
        GetPlayerProfile();
    }

    private void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received Statistics");
        foreach (var eachStat in result.Statistics)
        {
            //  Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);

            switch (eachStat.StatisticName)
            {
                case "Weapon":
                    stats.weapon = eachStat.Value;
                    break;

                case "Level":
                    stats.level = eachStat.Value;
                    break;

                case "Stars":
                    stats.stars = eachStat.Value;
                    break;

                case "Coins":
                    stats.coins = eachStat.Value;
                    break;

                case "Wins":
                    stats.wins = eachStat.Value;
                    break;

                case "Loss":
                    stats.loss = eachStat.Value;
                    break;

                case "Haki Level":
                    stats.hakilvl = eachStat.Value;
                    break;

                case "Haki Pills":
				stats.hakipills = eachStat.Value;
                    break;

                case "Speed Pills":
				stats.speedpills = eachStat.Value;
                    break;
            }
        }

    }

    // Build the request object and access the API
    /*   public void StartCloudUpdatePlayerStats()
       {
           PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
           {
               FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
               FunctionParameter = new
            {      weapon = data.weapon,
                   level = data.level,
                   stars = data.stars,
                   coins = data.coins,
                   wins = data.wins,
                   loss = data.loss,
                   hakilvl = data.hakilvl,
                   hakipills = data.hakipills,
                   speedpills = data.speedpills
               }, // The parameter provided to your function
               GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
           }, OnCloudUpdateStats, OnErrorShared);
       }
       // OnCloudHelloWorld defined in the next code block
       private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
       {
           // Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
          // Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
           JsonObject jsonResult = (JsonObject)result.FunctionResult;
           object messageValue;
           jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script
           Debug.Log((string)messageValue);
       }
       private static void OnErrorShared(PlayFabError error)
       {
           Debug.Log(error.GenerateErrorReport());
       } */
    #endregion

   
    public GameObject listingLeaderboard;
    private Transform Content;

#region Leaderboard

    public void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "Stars", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeadboard, OnErrorLeaderboard);
    }

    void OnGetLeadboard(GetLeaderboardResult result)
    {
       Content = GameObject.Find("Leaderboard Content").GetComponent<Transform>();
        //Debug.Log(result.Leaderboard[0].StatValue);
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(listingLeaderboard, Content);
            Leaderboard lb = tempListing.GetComponent<Leaderboard>();
            lb.playerText.text = player.DisplayName + " : " + player.StatValue.ToString();
        //    Debug.Log(player.DisplayName + ": " + player.StatValue);
        }
    }
    public void CloseLeaderboard()
    {
        for (int i = Content.childCount - 1; i >= 0; i--)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
    }
    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
#endregion

#region Player Data

    public void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myID,
            Keys = null
        }, OnUserDataSuccess, OnErrorLeaderboard);
    }

    void OnUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("Weapons"))
        {
            //Debug.Log("Weapons not set");
        }
        else
        {
            //Get the resutls of the requests and sends it to be converted to the all skins array.
            PresistentData.PD.WeaponsStringToData(result.Data["Weapons"].Value);
            Debug.Log("getting user data");

        }
    }

    public void SetPlayerData(string WeaponsData)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                //key value pair, saving the allskins array as a string to the playfab cloud
                {"Weapons", WeaponsData}
            }
        }, OnSetDataSuccess, OnErrorLeaderboard);
    }

    void OnSetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
    }
#endregion

}