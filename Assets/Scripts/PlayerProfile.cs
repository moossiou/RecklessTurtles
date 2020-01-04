using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerProfile : NetworkBehaviour {

	public static PlayerProfile PP;
    
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI txtCoins;
    public TextMeshProUGUI txtStars;
    public TextMeshProUGUI txtWins;
    public TextMeshProUGUI txtLoss;


    public GameObject EditButton;
	public GameObject LoginButton;
	public GameObject LogoutButton;
	public GameObject RegisterButton;
   
    public GameObject loginRegistrationPanel;
    public GameObject login;
    public GameObject registration;
  //  public PlayerStats stats;

	private PlayFabController pf;
    
	private void OnEnable()
	{
		if (PP == null)
		{
			PP = this;
		}
		else
		{
			if (PP != this)
			{
				Destroy(this.gameObject);
			}
		}
	}
 
    private void Start()
    {
        pf = PlayFabController.PFC;
            
    }

   public void Edit()
    {
    
    }

   

    public void Save()
    {
        /*    
           if (InputUsername.text != "")
           {
           PlayFabController.PFC.UpdateDisplayName();
           }
           else
           {
               Debug.LogWarning(" You need to inter a Username");
           }

       // SaveSystem.SaveData(data);
       PlayFabController.PFC.SetStats();
    //   Edited();*/

        PlayFabController.PFC.UpdateDisplayName();

    }

    private void Edited()
    {
      
    }


    private void Update()
    {
		
        //  data = pf.data;
		usernameText.text = pf.stats.username;
        txtCoins.text = "Coins: " + pf.stats.coins;
        txtStars.text = "Stars: " + pf.stats.stars;
        txtWins.text = "Wins: " + pf.stats.wins;
        txtLoss.text = "Loss: " + pf.stats.loss;
       
    }

    public void Login()
    {
		loginRegistrationPanel.SetActive(true);
        login.SetActive(true);
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void Register()
    {
		loginRegistrationPanel.SetActive(true);
        registration.SetActive(true);
    }

    public void Back (GameObject obj)
    {
        obj.SetActive(false);
		loginRegistrationPanel.SetActive(false);
    }


}
