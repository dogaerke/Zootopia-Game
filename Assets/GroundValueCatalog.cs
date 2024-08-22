using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZoneStatusNamespace.Triggers.GroundTriggerSystem;

public class GroundValueCatalog : ScriptableObject
{
    [SerializedDictionary("type", "Container")]
    public SerializedDictionary<string, GroundTriggerValueContainer> catalogDictionary;

    public List<VariableSetList> variableSetList;

    public void AddValue(string id, float value)
    {
        if (catalogDictionary.ContainsKey(id))
        {
            catalogDictionary[id].SetValue(value);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            
            return;//degeri guncelle
        }

        var container = new GroundTriggerValueContainer();

        var split = id.Split('_');
        //0. index map
        //1. index type
        //2. index upgrade count    
        foreach (var v in variableSetList)
        {
            if (split[1] != v.type) continue;
            container.SetSprite(v.icon);
            container.SetSpriteState(split[2].EndsWith("0") ? 0 : 1);
            container.Title = split[1];
            break;
        }
        
        catalogDictionary.Add(id, container);
        container.SetValue(value);
        //container.SetTitle(id);
    }

    public GroundTriggerValueContainer GetContainer(string id)
    {
        if (!catalogDictionary.ContainsKey(id))
            return null;//degeri guncelle

        return catalogDictionary[id];
    }
}
