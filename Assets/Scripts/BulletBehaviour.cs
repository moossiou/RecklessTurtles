using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public GameObject other;
    [SerializeField]
    private Rigidbody rb;
    public GameObject player;

    private void Awake()
    {
        DestroyObject(gameObject, 5f);
    }

    public void OnCollisionEnter(Collision hit)
    {
        foreach (ContactPoint contact in hit.contacts)
        {         
           other = contact.otherCollider.gameObject;
         
            player.GetComponent<PlayerShoot>().Hit(contact.point, contact.normal,gameObject);

            if (other.tag == "Rock")
            {
                other.GetComponent<DistractibleObjects>().Explode();
            }
              Destroy(gameObject);
        }
       
    }
 
}
