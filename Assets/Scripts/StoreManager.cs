using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager SM;


	private PlayFabController pf;
    public Button[] buyButtons;
    public Button[] selectButtons;

	public List<PlayerWeapon> weapons = new List<PlayerWeapon>();

    private void OnEnable()
    {
        StoreManager.SM = this;    
    }


    private void Start()
    {
        SetUpStore();
    }

    public void SetUpStore()
    {
			for (int i = 0; i < PresistentData.PD.weapons.Length; i++) {
			if (i != PlayFabController.PFC.stats.weapon && PresistentData.PD.weapons [i] == true) {
					selectButtons [i].interactable = true;
				} else {
					selectButtons [i].interactable = false;
				}         
				buyButtons [i].interactable = !PresistentData.PD.weapons [i];
			}
    }
  
	public void OnClickBuy(int index)
	{
		// check if have enough coins to buy
		if (pf.stats.coins >= weapons[index].price) {
			Buy (index);
		}else{
			Debug.Log ("YOU DONT HAVE ENOUGH COINS TO BUY THIS WEAPON");
		}
	}
    public void Buy(int index)
    {
        PresistentData.PD.weapons[index] = true;
		pf.SetPlayerData(PresistentData.PD.WeaponsDataToString());
		pf.stats.coins = pf.stats.coins - weapons [index].price;
        SetUpStore();

    }

    public void SelectWeapon(int index)
    {
		pf.stats.weapon = index;
		pf.SetStats();      
    }

    private void Update()
    {
		pf = PlayFabController.PFC;
        for (int i = 0; i < selectButtons.Length; i++)
        {
			if (i != pf.stats.weapon && PresistentData.PD.weapons[i] == true)
			{
                selectButtons[i].interactable = true;
            }
            else
            {
                selectButtons[i].interactable = false;
            }
        }
    }
}
