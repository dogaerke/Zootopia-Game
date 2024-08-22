using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBoxPositions : MonoBehaviour
{
    [SerializeField] private List<Transform> boxList;
    [SerializeField] private Transform boxParent;

    public List<Transform> GetBoxList()
    {
        return boxList;
    }

    void ShiftBoxes(int counter)
    {
        int index = 0;
        foreach (Transform box in boxParent)
        {
            if (index >= counter)
            {
                box.position = boxList[index - 1].position;
            }
            index++;
        }
    }
    
}
