using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{

    #region Player Data
private static string playerfilename = "/1902501005016.401018010";

    public static void SaveData(PlayerStats data)
    {      
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerfilename ;
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log( path); 

        formatter.Serialize(stream, data);
        stream.Close();           
    }

    public static PlayerStats LoadData()
    {
        string path = Application.persistentDataPath + playerfilename;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

           PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
            stream.Close();
            return data;

        } else
        {
            Debug.Log("file does not existe in " + path);           
            return new PlayerStats ();
        }
    }

    #endregion

    #region  Settings
    private static string settingsfilename = "/settings.data";

    public static void SaveSettings(SettingsData settingsdata)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + settingsfilename;
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log(path);

        formatter.Serialize(stream, settingsdata);
        stream.Close();
    }


    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + settingsfilename;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

           SettingsData settingsdata = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
            return settingsdata;

        }
        else
        {
            Debug.Log("file does not existe in " + path);
            return new SettingsData();
        }
    }
    #endregion 
}
