
using UnityEngine;

public class cameramovement : MonoBehaviour {

    public Transform player;
    public float smoothSpeed = 1.25f;
    public Vector3 offset;
   
  

    void FixedUpdate () {
         Vector3 desieredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desieredPosition, smoothSpeed);
            transform.position = smoothPosition;
        transform.LookAt(player);
	}
  
}
