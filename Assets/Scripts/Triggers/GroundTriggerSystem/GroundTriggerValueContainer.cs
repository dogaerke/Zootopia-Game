using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ZoneStatusNamespace.Triggers.GroundTriggerSystem
{
    [Serializable]
    public class GroundTriggerValueContainer
    {
        [SerializeField] private float moneyNeedForThisTrigger;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private SpriteState spriteState;
        [SerializeField] private Sprite _icon;
        private string _nameVal;

        public float MoneyNeedForThisTrigger => moneyNeedForThisTrigger;

        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public SpriteState SpriteState => spriteState;

        public string NameVal
        {
            get => _nameVal;
            set => _nameVal = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public void SetValue(float value)
        {
            moneyNeedForThisTrigger = value;
        }

        public void SetTitle(string t)
        {
            title = t;
        }

        public void SetDescription(string d)
        {
            description = d;
        }

        public void SetSprite(Sprite sprite)
        {
            _icon = sprite;
        }

        public void SetSpriteState(int level)
        {
            if (level == 0)
            {
                spriteState = SpriteState.NEW;
                return;
            }
            spriteState = SpriteState.UPGRADE;
        }
    }
}