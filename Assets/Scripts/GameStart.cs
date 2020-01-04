
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



public class GameStart : NetworkBehaviour {

   [SyncVar]
   public PlayerStats enemyStats;
   
 

    public override void OnStartLocalPlayer()
    {
        StartCoroutine(SendStatsAfterTime(2f));
    }

    private IEnumerator SendStatsAfterTime(float delay)
    {
        if (isLocalPlayer)
        {
           yield return new WaitForSeconds(delay);
           SendStats();
        }
       
    }

    void SendStats()
    {
        if (isLocalPlayer)
        {
            CmdSend();
        }
    }

    #region Sending Players Stats (Messaging System )

    public class MyMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public string usernametosend;
        public int coinstosend;
        public int lvltosend;
        public int starstosend;
    }



    short MyMsgId = 1001;

    public override void OnStartClient()
    {
       
        NetworkManager.singleton.client.RegisterHandler(MyMsgId, OnMyMsg);    
    }

    [Command]
  public  void CmdSend()
    {
        
       PlayerStats stats = PlayFabController.PFC.stats;
       var msg = new MyMessage();
 
        msg.usernametosend = stats.username;
        msg.coinstosend = stats.coins;
        msg.starstosend = stats.stars;
        msg.lvltosend = stats.level;

        msg.netId = netId;

     
       
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


     void DoStuff(string usernamerecived, int coinsrecived, int starsrecived, int lvlrecived)
    {      
        enemyStats.username = usernamerecived;
        enemyStats.coins = coinsrecived;
        enemyStats.stars = starsrecived;
        enemyStats.level = lvlrecived;            
    }

        

    static void OnMyMsg(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MyMessage>();
       
        var plyr = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < plyr.Length; i++)
        {
            if (plyr[i].GetComponent<NetworkIdentity>().netId != msg.netId)
            {
                plyr[i].GetComponent<GameStart>().DoStuff(msg.usernametosend, msg.coinstosend, msg.starstosend, msg.lvltosend);
                Debug.Log(plyr[i]);
            }
        }

    }

    #endregion

}

