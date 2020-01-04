using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;


   

    void Awake()
    {
     if ( instance != null)
        {
            Debug.Log("there is more than one Game Manager in the scene");
        } else
        {
            instance = this;
        }
    }

    #region Player traking 
    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

   
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = "Player " + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static Player GetPlayer(string _playerID)
    {

        if (!players.ContainsKey(_playerID))
        { Debug.Log("hey! this played id is no good! " + _playerID); return null; }

        else { return players[_playerID]; }
    }

    /*  public static Player[] GetAllPlayers ()
      {
          return players.Values.ToArray();

      }

      */
   
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }
    // void OnGUI()
    // {

    //   GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //   GUILayout.BeginVertical();

    //   foreach (string _playerID in players.Keys)
    //      {
    //          GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
    // /      }

    //   GUILayout.EndVertical();
    //s    GUILayout.EndArea();
    //   }
    #endregion
}
