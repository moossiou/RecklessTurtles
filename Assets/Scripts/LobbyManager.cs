
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;




public class LobbyManager : NetworkLobbyManager
{
    public static LobbyManager LM;

	// singleton
    private void OnEnable()
    {
        LM = this;
    }
    private void Start()
    {
        dontDestroyOnLoad = true;    
    } 

    public void Play()
        {
      
        StartMatchMaker();
        ListMatches();        
        }

        
	// show the available matches
        void ListMatches()
        {
            this.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);
        }

        public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            base.OnMatchList(success, extendedInfo, matchList);


        if (!success)  // if list matches failled
        {
            Debug.Log(" on match list failled " + extendedInfo);
            Debug.Log("poor connection");
            dontDestroyOnLoad = false;
            StopMatchMaker();      
        }
		else   // if list matches success
        {
			if (matchList.Count > 0) // if there is an existing matche JOIN
            {
                JoinMatch(matchList[0]);
            }else // if there is no matches CREATE one
            { 
                 CreateMatch();
             }
        }
       }

        void JoinMatch(MatchInfoSnapshot firstMatch)
        {
            this.matchMaker.JoinMatch(firstMatch.networkId, "", "", "", 0, 0, OnMatchJoined);
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchJoined(success, extendedInfo, matchInfo);
            if (!success)
            {
            Debug.Log("join match failed : poor connection");
            StopMatchMaker();          
            dontDestroyOnLoad = false;

        }
            else
            {         
			// check if all players are in the match
             StartCoroutine(DetectMatch(3f));      
             }
        }

    void CreateMatch()
        {
          matchMaker.CreateMatch("M", 2, true, "", "", "", 0, 0, OnMatchCreate);          
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            if (!success)
            {
            dontDestroyOnLoad = false;
            }
            else
            {          
			// check if all players are in the match
            StartCoroutine(DetectMatch(10f));
            }
        }

    public override void OnLobbyServerSceneChanged(string LobbyScene)
    {
        base.OnLobbyServerSceneChanged(LobbyScene);
        dontDestroyOnLoad = false;
    }

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Poor Connection");
        }
    }

	// this void makes the match making more active BUT if you have a slow device or poor connection this may cause a problem that the match making will never happend 
    private IEnumerator DetectMatch(float delay) 
    {
        yield return new WaitForSeconds(delay);     
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 0)
        {
            Debug.Log("No players  :  try again");
            Stop();
            StartCoroutine(Play(1.5f));
        }
    }

    private IEnumerator Play(float delay)
    {
        yield return new WaitForSeconds(delay);
        Play();
    }

    #region Disconnecting


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("the other player diconnect");
        Stop();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Stop();
    }

  

    public void Drop(NetworkID netid, NodeID nodeid)
    {
         singleton.matchMaker.DropConnection(netid, nodeid, 0, OnDropConnection);
    }

    public override void OnDropConnection(bool success, string extendedInfo )
    {
        base.OnDropConnection(success, extendedInfo );

        if (success)
        {
           Debug.Log("drop from " + matchInfo.networkId);
            
           DestroyMatch(singleton.matchInfo.networkId);
           
        }
    }

   public void DestroyMatch (NetworkID netid)
    {
           singleton.matchMaker.DestroyMatch(netid, 0, OnDestroyMatch);  
    }
    
    public override void OnDestroyMatch(bool success, string extendedInfo )
    {
        base.OnDestroyMatch(success, extendedInfo);
        if (success)
        {
            //   Network.Disconnect(); 
            singleton.StopClient();
            singleton.StopServer();
            singleton.StopHost();
            singleton.StopMatchMaker();
        }
        if (!success)
        {
            Debug.Log("destroy match  failled  " );
           
         //   Network.Disconnect();
            singleton.StopClient();
            singleton.StopServer();
            singleton.StopHost();
            singleton.StopMatchMaker();

            //  Shutdown();
        }
    }
   
    public void Stop()
    {
        if (singleton.matchInfo != null)
        {
          DestroyMatch(singleton.matchInfo.networkId);
        }
        else
        {
            singleton.StopClient();
            singleton.StopServer();
            singleton.StopHost();
            singleton.StopMatchMaker();
        }
    }
   

    #endregion


}

