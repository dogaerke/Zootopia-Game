using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierIdlePoint : MonoBehaviour
{
    private void OnEnable()
    {
        HelperManager.Instance.RegisterCarrierHelperIdlePoint(transform);
    }
}
