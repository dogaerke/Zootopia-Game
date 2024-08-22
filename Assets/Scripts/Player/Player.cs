using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] public List<Transform> stackPoints;
    [SerializeField] public Transform boxParent;

    /*private int _experience;
    private int _playerLevel;*/

    public int stackIndex = 0;
    
    public static Player Instance { get; private set; }
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("All Prefs Deleted");
            PlayerPrefs.DeleteAll();

        }
    }

    public bool IsPlayerStop()
    {
        float horizontal = dynamicJoystick.Horizontal;
        float vertical = dynamicJoystick.Vertical;

        Vector3 newVec = new Vector3(horizontal, 0f, vertical).normalized;

        if (newVec.magnitude < 0.01f)
        {
            return true;
        }

        return false;
    }

    public void ShiftBoxes(int counter)
    {
        int index = 0;
        foreach (Transform box in boxParent)
        {
            if (index >= counter)
            {
                box.position = stackPoints[index - 1].position;
            }
            index++;
        }
    }

    
}
