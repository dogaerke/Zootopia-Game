using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZoneStatusNamespace.Triggers.GroundTriggerSystem
{
    [Serializable]
    public class VariableSetList
    {
        public int keyIndex;
        public string type;
        public Sprite icon;
        public List<GameObjectProperties> titleValuesList;
    }
}