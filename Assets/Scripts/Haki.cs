using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haki : MonoBehaviour {

    public Material[] haki;
    public GameObject[] objects;
    public Material[] originalMaters;
    public GameObject character;

    public void Start()
    {
           for (int i = 0; i < objects.Length; i++)
            {
                originalMaters[i] = objects[i].GetComponent<Renderer>().material;
            }
    }

    public IEnumerator OnHaki (float doration)
    {
        yield return new WaitForSeconds(doration);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Renderer>().material = originalMaters[i];                  
        }
    }

    public void SetHakiOn(int hakilvl)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Renderer>().material = haki[hakilvl];
        }
        StartCoroutine(OnHaki(3));
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetHakiOn(0);
        }
      
        

    }
}
