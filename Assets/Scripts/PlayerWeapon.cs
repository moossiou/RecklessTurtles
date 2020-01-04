using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour {

    public new string name;
    public float damage;
    public float range;


    public float fireRate;
    public GameObject graphics;
    public Vector3 offset;

    public GameObject firePoint;

    public GameObject bulletGraphics;
    public float bulletSpeed ;

	public int price;
   
}


