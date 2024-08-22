using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePoint : MonoBehaviour
{
    private void OnEnable()
    {
        HelperManager.Instance.RegisterCleanerHelperIdlePoint(transform);
    }
}
