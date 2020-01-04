
using UnityEngine;
using UnityEngine.Networking;



public class LobbyPlayer : NetworkLobbyPlayer
{
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SendReadyToBeginMessage();
        Debug.Log("Ready");

    }
  
}