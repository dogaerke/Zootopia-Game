using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    
    
    public void IncreaseMoney()
    {
        CurrencyManager.Instance.IncreaseMoney(1000);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
    }

    public void IncreaseSpeed()
    {
        movement.Speed += 100;
    }

    public void DecreaseSpeed()
    {
        movement.Speed -= 100;
    }
}
