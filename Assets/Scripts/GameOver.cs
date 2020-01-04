using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class GameOver : NetworkBehaviour {

  
   
    [SerializeField]
    private GameObject GameOverPanel;

    [SerializeField]
    private GameObject PlayerUI;

    [SerializeField]
    private Text GameOverText;

    [SerializeField]
    private PlayerStats EnemyStats;

    private PlayFabController pf;


    private void Start()
    {
        pf = PlayFabController.PFC;
    }

    [SyncVar]
    private bool _isWinner;
    public bool isWinner
    {
        get { return _isWinner; }
        protected set { _isWinner = value; }
    }
   
   
    public void Win()
    {
        if (isLocalPlayer)
        {
            isWinner = true;
            Debug.Log(transform.name + " Win");

            // score calculation
            pf.stats.wins ++;
			pf.stats.coins = pf.stats.coins + EnemyStats.coins / 10 + 20;
			pf.stats.stars = pf.stats.stars + EnemyStats.stars / 10 + 10;

            pf.SetStats();
        
			StartCoroutine(GameEnd(1f));
            GameOverText.text = "YOU WIN";

        }
      
    }


    [Client]
    public void Lose()
    {
       
        GameObject[] uis = GameObject.FindGameObjectsWithTag("UI");
        foreach(GameObject ui in uis) {
             ui.SetActive(false);
                                      }
       

        if (isLocalPlayer)
            {
                isWinner = false;
            // score calculation
                Debug.Log(transform.name + " loss");
			pf.stats.coins = pf.stats.coins - EnemyStats.coins / 10 + 10;
			pf.stats.stars = pf.stats.stars - EnemyStats.stars / 10 + 5;
                pf.stats.loss ++;


            pf.SetStats();
              
      
                CmdSend();           

		    	StartCoroutine(GameEnd(1f));
                 GameOverText.text = "YOU LOSE";

    
        }
       
     
    }
    #region Messaging System


    public class MyMessage : MessageBase
    {
        public NetworkInstanceId netId;               
    }

    short MyMsgId = 1000;

    public override void OnStartClient()
    {
        // this should be somewhere else..
        
        NetworkManager.singleton.client.RegisterHandler(MyMsgId, OnMyMsg);
    }


    [Command]
    void CmdSend()
    {
        var msg = new MyMessage();
        msg.netId = netId;

        ///  base.connectionToClient.Send(MyMsgId, msg);
        if (NetworkServer.connections.Count == 2)
        {
            if (connectionToClient.connectionId == 0)
            {
                Debug.Log(" Send command ");
                NetworkServer.SendToClient(1, MyMsgId, msg);
            }

            if (connectionToClient.connectionId == 1)
            {
                Debug.Log(" Send command ");
                NetworkServer.SendToClient(0, MyMsgId, msg);
            }
        }

        if (NetworkServer.connections.Count == 3)
        {
            if (connectionToClient.connectionId == 1)
            {
                Debug.Log(" Send command ");
                NetworkServer.SendToClient(2, MyMsgId, msg);
            }

            if (connectionToClient.connectionId == 2)
            {
                Debug.Log(" Send command ");
                NetworkServer.SendToClient(1, MyMsgId, msg);
            }
        }
    }



    public void DoStuff()
    {
        Debug.Log("do stuff");
        Win();

    }


    static void OnMyMsg(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MyMessage>();

        Debug.Log("on my msg");
        Debug.Log( "message id " + msg.netId);
         
        var plyr = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < plyr.Length; i++)
        {
            if (plyr[i].GetComponent<NetworkIdentity>().netId != msg.netId)
            {
                plyr[i].GetComponent<GameOver>().DoStuff();
                Debug.Log(plyr[i]);
            }
        }             
    }

    public void GoBackToLobby()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        LobbyManager.LM.Stop();
    }

    private IEnumerator GameEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOverPanel.SetActive(true);
    }
    #endregion 
}
