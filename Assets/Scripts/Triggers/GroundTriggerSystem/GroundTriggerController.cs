using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ZoneStatusNamespace.Triggers.GroundTriggerSystem
{
    public class GroundTriggerController : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private float moneyNeedForThisTrigger;
        [SerializeField] private GroundValueCatalog data;
        [SerializeField] private GroundTriggerHandler handler;
        
        private void Start()
        {
            id = handler.Id;
            var container = data.GetContainer(id);
            handler.Initialize(container.MoneyNeedForThisTrigger, id, container.Icon, container.SpriteState, container.Title);
        }

        [Button("AddValue", EButtonEnableMode.Editor)]
        private void AddValue()
        {
            handler.SetID(id);
            data.AddValue(handler.Id, moneyNeedForThisTrigger);
            gameObject.name = id;
            
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(handler);
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}