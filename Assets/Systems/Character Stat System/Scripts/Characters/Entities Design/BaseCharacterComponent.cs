using Codice.Client.BaseCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


namespace Stats.Entities
{
    public partial class BaseCharacterComponent : IComponentData
    {
        private string _name;
        private int _level;
        private uint _freeExp;
        private Attributes[] _primaryAttribute;
        private Vital[] _vital;
        private Stat[] _stats;
        private Abilities[] _ability;
        private Elemental[] _ElementalMods;
        public bool InPlay;
        public bool InvincibleMode;

        [Range(0, 9999)]
        public int CurHealth;

        [Range(0, 9999)]
        [SerializeField] int maxHealth;
        public int MaxHealth { get { return MaxHealthMod + maxHealth; } set { maxHealth = value; } }
        public int MaxHealthMod { get; set; }
        [Range(0, 9999)]
        public int CurMana;
        [Range(0, 9999)]
        [SerializeField] int maxMana;
        public int MaxMana { get { return maxMana + MaxHealthMod; } set { maxMana = value; } }
        public int MaxManaMod { get; set; }
        public bool Dead => !InvincibleMode && CurHealth <= 0;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public uint FreeExp
        {
            get { return _freeExp; }
            set { _freeExp = value; }
        }
        public void AddExp(uint exp)
        {
            _freeExp += exp;
            CalculateLevel();
        }

        public void CalculateLevel()
        {

            // TODO need to add logic here

        }

        public void Init()
        {
            _name = string.Empty;
            _level = 0;
            _freeExp = 0;
            _primaryAttribute = new Attributes[Enum.GetValues(typeof(AttributeName)).Length];
            _vital = new Vital[Enum.GetValues(typeof(VitalName)).Length];
            _stats = new Stat[Enum.GetValues(typeof(StatName)).Length];
            _ability = new Abilities[Enum.GetValues(typeof(AbilityName)).Length];
            // _ElementalMods = new Elemental[Enum.GetValues(typeof(Elements)).Length];
            SetupPrimaryAttributes();
            SetupVitals();
            SetupStats();
           // SetupAbilities();

            CurHealth = MaxHealth;
            CurMana = MaxMana;
#if !UNITY_EDITOR
            InvincibleMode = false;
#endif

#if UNITY_EDITOR
            if (InvincibleMode)
                Debug.LogWarning($"This Character {Name} is in Invincible Mode and will not take Damage");
#endif

            // SetupElementalMods();
        }

        private void SetupPrimaryAttributes()
        {
            for (int cnt = 0; cnt < _primaryAttribute.Length; cnt++)
            {
                _primaryAttribute[cnt] = new Attributes();
            }
        }
        public Attributes GetPrimaryAttribute(int index)
        {
            return _primaryAttribute[index];
        }
        private void SetupVitals()
        {
            for (int cnt = 0; cnt < _vital.Length; cnt++)
                _vital[cnt] = new Vital();

            SetupVitalBase();
            SetupVitalModifiers();
        }
        public Vital GetVital(int index)
        {
            return _vital[index];
        }
        public Stat GetStat(int index)
        {
            return _stats[index];
        }
        public Abilities GetAbility(int index)
        {
            return _ability[index];
        }
        private void SetupStats()
        {
            for (int cnt = 0; cnt < _stats.Length; cnt++)
                _stats[cnt] = new Stat();
            SetupStatsBase();
            SetupStatsModifiers();
        }
        public void StatUpdate()
        {
            for (int i = 0; i < _vital.Length; i++)
                _vital[i].Update();
            for (int j = 0; j < _stats.Length; j++)
                _stats[j].Update();
            //for (int i = 0; i < _ability.Length; i++)
            //    _ability[i].Update();

            CurHealth = MaxHealth = GetVital((int)VitalName.Health).AdjustBaseValue;
            CurMana = MaxMana = GetVital((int)VitalName.Mana).AdjustBaseValue;
        }
    }
}