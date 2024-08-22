using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionistPrefs : MonoBehaviour
{
    private void OnEnable()
    {
        HelperManager.Instance.receptionist = this;
    }
}
