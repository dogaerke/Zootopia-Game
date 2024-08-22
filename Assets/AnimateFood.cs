using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFood : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    public void SetFalse()//Running through event
    {
        parent.SetActive(false);
    }
}
