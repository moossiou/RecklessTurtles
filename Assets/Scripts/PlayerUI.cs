using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : NetworkBehaviour {

    [SerializeField]
    private TextMeshProUGUI myName;
    [SerializeField]
    private TextMeshProUGUI enemyName;
    [SerializeField]
    private Player player;
    [SerializeField]
    private RectTransform playerHealthBar;
    [SerializeField]
    private RectTransform enemyHealthBar;
    
    [SerializeField]
    private RectTransform joystick;
    public Vector3 startPoint1;
    public Quaternion newRotation;

    [SerializeField]
    private GameObject longClick;

    private void Start()
    {
        if (transform.position != startPoint1)
        {
            joystick.transform.rotation = newRotation;
     }
              
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            myName.text = player.stats.username; 
            string name = GetComponent<GameStart>().enemyStats.username;
            enemyName.text = name;
        }
        SetHealthAmount();

        if (PlayFabController.PFC.stats.weapon > 1)
        {
            longClick.SetActive(true);
        }
    }

	void SetHealthAmount ()
    {
        if (isLocalPlayer)
        {
            float amount = player.currentHealth * 0.01f;
            playerHealthBar.localScale = new Vector3(amount, 1f, 1f);

        } else
        {          
            float amount = player.currentHealth * 0.01f;
           enemyHealthBar.localScale = new Vector3(amount, 1f , 1f);

        }
    }
	
}
