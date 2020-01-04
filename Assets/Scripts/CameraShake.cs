using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public IEnumerator Shake (float doration, float macins)
    {
        Vector3 originalepos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed <= doration)
        {
            float x = Random.Range(-1f, 1f) * macins;
            float y = Random.Range(-1f, 1f) * macins;
            transform.localPosition = new Vector3(x, y, originalepos.z);
            elapsed += Time.deltaTime;
           yield return null;
        }
        transform.localPosition = originalepos;
    }
}
