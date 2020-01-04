using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using DigitalRuby.LightningBolt;
using UnityEngine.UI;



[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    public GameObject weapon;
    private Laser laser;
    private LightningBoltScript lightninig;
    public PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    private int weaponIndex;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject grenadepre;
    public GameObject longclickimage;

    void Start()
    {
		
        if (weapon == null)
        {
            Debug.LogError("PlayerShoot : No weapon refrenced!");
            this.enabled = true;
        }
        weaponManager = GetComponent<WeaponManager>();
        weaponIndex = PlayFabController.PFC.stats.weapon;
    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

		// check if the current weapon is the laser cannon
        if (weapon.GetComponentInChildren<Laser>() != null)
        {
            laser = weapon.GetComponentInChildren<Laser>();
			// the long click image is the red cyrcle aroud the shoot button
            longclickimage.SetActive(true);
        }

		// check if the current weapon is the Lightning cannon
        if (weapon.GetComponentInChildren<LightningBoltScript>() != null)
        {
            longclickimage.SetActive(true);
            lightninig = weapon.GetComponentInChildren<LightningBoltScript>();
            lightninig.playershoot = this;
            lightninig.Start();
            lightninig.Update();
        }
        
    }
   
    public void Shoot()
    {
        if (!isLocalPlayer)
            return;
 
          Debug.Log("SHOOT");
   
        switch (weaponIndex) // check what type of weapon 
        {                  
            case 2:
			CmdShootLaser(); // if laser cannon
                break;
            case 3:
                CmdShootLightning(); // if lightning cannon 
                break;

            default:
                CmdOnShoot(); // if the default cannon or the tank cannon
                break;
        }
    }

    #region Deafult Weapons Shooting

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
        RpcThrowTheBullet();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzeleFlash.Play();
    }

    [ClientRpc]
    void RpcThrowTheBullet()
    {
        GameObject bullet = Instantiate(currentWeapon.bulletGraphics, weapon.transform.position, weapon.transform.rotation);

        bullet.GetComponent<BulletBehaviour>().player = gameObject;

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider coll in colliders)
        {
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), coll);
        }

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * currentWeapon.bulletSpeed, ForceMode.Impulse);

    }

    #endregion
 
    #region Hit Effect

    public void Hit(Vector3 hitPostion, Vector3 hitNormal, GameObject bullet)
    {
        Shot(hitPostion, hitNormal, bullet);
    }

    [Command]
    public void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {

        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 0.2f);
    }
    #endregion 

    #region Send Cmd/Rpc for Laser/Lightning
    [Command]
    void CmdShootLightning()
    {
        RpcShootLightning();
    }

    [ClientRpc]
    void RpcShootLightning()
    {
        lightninig.enabled = true;
        lightninig.Shoot();
    }


    [Command]
    void CmdShootLaser()
    {
        RpcShootLaser();
    }

    [ClientRpc]
    void RpcShootLaser()
    {
        laser.Shoot();
    }
    #endregion

    public void OnPointerUp()
    {
        if (!isLocalPlayer)
            return;

        switch (weaponIndex)
        {
            case 2:
                CmdDisableLaser();
                break;
            case 3:
                CmdDisableLightning();
                break;
        }
   }

    #region   Disable Laser/Lightning 
    [Command]
    void CmdDisableLaser()
    {
        RpcDisableLaser();
    }

    [ClientRpc]
    void RpcDisableLaser()
    {
          laser.Disable();      
    }

    [Command]
    void CmdDisableLightning()
    {
        RpcDisableLightning();
    }

    [ClientRpc]
    void RpcDisableLightning()
    {
        weapon.GetComponentInChildren<LineRenderer>().enabled = false;
        lightninig.enabled = false;
    }
    #endregion

    #region Taking a Shot

    void Shot (Vector3 hitPostion, Vector3 hitNormal, GameObject bullet)
    {
        if (!isLocalPlayer)
            return;
       
       GameObject other = bullet.GetComponent<BulletBehaviour>().other;

        if (other.tag == "Player")
        {
            CmdPlayerShot(other.name, currentWeapon.damage);
        }
        CmdOnHit(hitPostion, hitNormal);
    }

    [Command]
    public void CmdPlayerShot(string _playerID, float _damage)
    {
        //  Debug.Log(_playerID + " has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
    #endregion

    #region Shoot Grenade
    public void Grenade()
    {
        if (!isLocalPlayer)
            return;
        CmdThrowGrenade();
    }
    [Command]
    void CmdThrowGrenade()
    {
        RpcThrowGrenade();
    }

    [ClientRpc]
    void RpcThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadepre, transform.position, transform.rotation);

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider coll in colliders)
        {
            Physics.IgnoreCollision(grenade.GetComponent<Collider>(), coll);
        }

        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);

    }
    #endregion

}
