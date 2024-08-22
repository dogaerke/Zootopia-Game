using UnityEngine;

public static class PlayerPrefsHandler
{
    public static float GetPlayerMoney()
    {
        var keyName = "PlayerMoney";
        if (PlayerPrefs.HasKey(keyName)) return PlayerPrefs.GetFloat(keyName);
        SetPlayerMoney(25f);
        return 25f;

    }

    public static float GetRoomUpdaterMoneyAmount(string keyName)
    {
        var modifiedKeyName = keyName + "_Amount";
        if (!PlayerPrefs.HasKey(modifiedKeyName))
        {
            PlayerPrefs.SetFloat(modifiedKeyName, 0f);
            return 0f;
        }
        return PlayerPrefs.GetFloat(modifiedKeyName);
    }
    
    public static float GetCleanerAmount(string keyName)
    {
        var modifiedKeyName = keyName + "_Amount";
        if (!PlayerPrefs.HasKey(modifiedKeyName))
        {
            PlayerPrefs.SetFloat(modifiedKeyName, 0f);
            return 0f;
        }
        return PlayerPrefs.GetFloat(modifiedKeyName);
    }

    public static void SetRoomUpdateMoneyAmount(string name, float amount)
    {
        var modifiedName = name + "_Amount";
        PlayerPrefs.SetFloat(modifiedName, amount);
    }
    
    public static void SetCleanerAmount(string name, float amount)
    {
        var modifiedName = name + "_Amount";
        PlayerPrefs.SetFloat(modifiedName, amount);
    }
    
    public static void SetPlayerMoney(float moneyAmount)
    {
        var keyName = "PlayerMoney";
        PlayerPrefs.SetFloat(keyName, moneyAmount);
    }
}
