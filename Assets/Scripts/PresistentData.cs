using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresistentData : MonoBehaviour {

    public static PresistentData PD;

    public bool[] weapons;

  

    private void OnEnable()
    {
        PD = this;
    }

    public void WeaponsStringToData(string weaponsIn)
    {
        for (int i = 0; i < weaponsIn.Length; i++)
        {
            if (int.Parse(weaponsIn[i].ToString()) > 0)
            {
                weapons[i] = true;
            }
            else
            {
                weapons[i] = false;
            }
        }
     //   StoreManager.SM.SetUpStore();
    }


    public string WeaponsDataToString()
    {
        string toString = "";
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == true)
            {
                toString += "1";
            }
            else
            {
                toString += "0";
            }
        }
        return toString;
    }
}
