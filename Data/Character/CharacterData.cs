using System;

namespace Core.Services
{
    [Serializable]
    public class CharacterData
    {
        public string Id;
        public string Name;
        public string ClassName;

        public int Level = 1;
        public int Experience = 0;
        public int Prestige = 0;

        public int HP = 100;
        public int BonusHP = 0;
        public int Damage = 20;
        public int BonusDamage = 0;

        public float LifeSteal = 0f;  
        public float DoublePunch = 0f;  
        public float CritChance = 0f;  
        public float CoinBonus = 0f; 

        public int SkinIndex = 0;

        public int Kills = 0;
        public int GamesPlayed = 0;

        public bool HardcoreMode = false;
        public bool IsActive = false;

        public string CreatedAt = DateTime.Now.ToString("o");

        public int TotalHP => HP + BonusHP;
        public int TotalDamage => Damage + BonusDamage;
        public int ExperienceToNextLevel => Level * 100;
    }
}