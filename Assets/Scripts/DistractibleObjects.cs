using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractibleObjects : MonoBehaviour {

    public GameObject destroyedObj;


   public void Explode()
    {
        GameObject.Instantiate(destroyedObj, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
