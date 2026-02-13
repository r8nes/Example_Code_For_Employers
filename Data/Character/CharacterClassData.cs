using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    [CreateAssetMenu(fileName = "CharacterClass", menuName = "TOG/Character Class")]
    public class CharacterClassData : ScriptableObject
    {
        [Header("Base")]
        public string ClassName;
        public string ClassDescription;
        public Sprite ClassSprite;

        [Header("Base UI values")]
        [Range(0f, 1f)] public float BaseHealth = 0.5f;
        [Range(0f, 1f)] public float BaseDamage = 0.5f;
        [Range(0f, 1f)] public float BaseDifficulty = 0.3f;

        [Header("Basic values")]
        public int StartHP = 100;
        public int StartDamage = 20;

        [Header("Уровневые умения")]
        public List<string> LevelSkills = new();

        [Header("Waeknbes")]
        public string Weakness;
    }
}