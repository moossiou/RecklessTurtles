using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField]
    private float delayTime = 2f;

    [SerializeField]
    private float radius = 10;

    [SerializeField]
    private float force = 700f;

    [SerializeField]
    private int damage = 20;

    [SerializeField]
    private GameObject gronadeExplosion;

    void Start()
    {
        StartCoroutine(Explode(delayTime));
    }

    private IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explosion();
    }

    private void Explosion()
    {
        GameObject _gfx = (GameObject)Instantiate(gronadeExplosion, transform.position, gronadeExplosion.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearObject in colliders)
        {
            DistractibleObjects disobjects = nearObject.GetComponent<DistractibleObjects>();
            if (disobjects != null)
            {
                disobjects.Explode();
            }

            Rigidbody rb = nearObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

           
      if (nearObject != null)
        {
            if (nearObject.tag == "Player")
            {     
                nearObject.GetComponent<Player>().RpcTakeDamage(damage);
            }                        
        }
    }
        Destroy(_gfx, 2f);
        Destroy(gameObject);
    }
}
