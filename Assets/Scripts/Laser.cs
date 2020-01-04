using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Laser : NetworkBehaviour {


   public LineRenderer line;
    private PlayerShoot playershoot;

    [SerializeField]
    private LayerMask mask;


    void Start () {
     
         line.enabled = false;
    playershoot = GetComponentInParent<PlayerShoot>();

    }
	
  
   public void Shoot()
    {    
            StopCoroutine("FireLaser");    
            StartCoroutine("FireLaser");
    }
  
    public void Disable()
    {
        line.enabled = false;
    } 
   
    public  IEnumerator FireLaser()
    {
             line.enabled = true;
     while (line.enabled == true)
        {
          //  laser.material.mainTextureOffset = new Vector2(2, Time.time);
            
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            line.SetPosition(0, ray.origin);

            if(Physics.Raycast(ray, out hit, 100, mask))
            {
                line.SetPosition(1, hit.point);
                playershoot.CmdOnHit(hit.point, hit.normal);
                if(hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log(hit.collider.name);
                    playershoot.CmdPlayerShot(hit.collider.name, playershoot.currentWeapon.damage);

                }
            }
            else
            {
               line.SetPosition(1, ray.GetPoint(100));
            }
          
          yield return null;
       }

       
    }
}
