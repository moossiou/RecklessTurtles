using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class WeaponManager : NetworkBehaviour {
    
	public static WeaponManager WM;
    [SerializeField]
    private GameObject weaponHolder;
    
    public List<PlayerWeapon> weapons = new List<PlayerWeapon>();

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    public int weapoindex;


	public void OnEnabled(){
		WeaponManager.WM = this;
	}

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (currentWeapon == null)
            {
                SpawnWeapon();
            }
        }
    }


   
    private void SpawnWeapon()
    {
        // making sure all players are in the game so weapons get spawed in all players scenes
        GameObject[] plys = GameObject.FindGameObjectsWithTag("Player");
        if (plys.Length == LobbyManager.LM.minPlayers)
        {
            StartWeapon();
        }
    }
   
    public void StartWeapon()
    {
        Debug.Log("spawn weapon");
       weapoindex = PlayFabController.PFC.stats.weapon;
        CmdWeapon(PlayFabController.PFC.stats.weapon);      
    }


    [Command]
    void CmdWeapon(int index)
    {
        RpcEquipWeapon(index);
    }


     public PlayerWeapon GetCurrentWeapon ()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    [ClientRpc]
    void RpcEquipWeapon ( int index )
    {
       
        PlayerWeapon _weapon = weapons[index];
        currentWeapon = _weapon ;
    
        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.transform.position + _weapon.offset, weaponHolder.transform.rotation);
     
        weaponIns.GetComponent<Transform>().parent = weaponHolder.transform;
        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
      
        if (isLocalPlayer)
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer("Weapon"));

     if (weaponHolder.transform.childCount > 1)
        {
            Destroy(weaponHolder.transform.GetChild(0).gameObject);
        }
       

    }
  
}
