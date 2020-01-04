using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    public GameObject profilePanel;
    public GameObject lobbyPanel;
    public GameObject settingsPanel;
    public GameObject Event;


   
   

    public void Play()
    {
		LobbyManager.LM.Play();
    }

   
	// this void opens all panels in the lobby scene
   public void OpenPanel (GameObject panel)
    {
        panel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

	// this void closes all panels in the lobby scene
    public void Back(GameObject panel)
    {
        lobbyPanel.SetActive(true);
        panel.SetActive(false);
    }

    private void Update()
    {
		// this makes sur that even if the player clicks the play button more than once no problems will happend
		if (LobbyManager.LM.matchMaker)
        {
            Event.SetActive(false);
        }
        else
        {
            Event.SetActive(true);
        }
    
    }

	// get leaderboad data 
    public void GetLeaderboard()
    {
        PlayFabController.PFC.GetLeaderboard();
    }

    public void ClearLeaderboard()
    {
        PlayFabController.PFC.CloseLeaderboard();
    }
   
}