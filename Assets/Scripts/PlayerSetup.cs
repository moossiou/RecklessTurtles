using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {  

  //  [SerializeField]
 //   Behaviour[] componentsToDisable;

    [SerializeField]
    GameObject[] gameObjectsToDisableOnRemotePlayer;
    [SerializeField]
    GameObject[] gameObjectsToDisableOnLocalPlayer;

    [SerializeField]
   string remoteLayerName = "RemotePlayer";

  public  Camera sceneCamera;
   
    public void Start()
    {
        if (!isLocalPlayer)
        {
          //  DisableComponents();
            DisableGameObjectsOnRemotePlayer();
            AssingRemotePlayer();
        }
        else
        {
            DisableGameObjectsOnLocalPlayer();
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    void AssingRemotePlayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);  
    }

    void DisableGameObjectsOnRemotePlayer ()
    {
        for (int i = 0; i < gameObjectsToDisableOnRemotePlayer.Length; i++)
        {
            gameObjectsToDisableOnRemotePlayer[i].SetActive(false);
        }
    }

    void DisableGameObjectsOnLocalPlayer()
    {
        for (int i = 0; i < gameObjectsToDisableOnLocalPlayer.Length; i++)
        {
            gameObjectsToDisableOnLocalPlayer[i].SetActive(false);
        }
    }

    /*   void DisableComponents()
       {
           for (int i = 0; i < componentsToDisable.Length; i++)
           {
               componentsToDisable[i].enabled = false;
           }
      }  */

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }

}
