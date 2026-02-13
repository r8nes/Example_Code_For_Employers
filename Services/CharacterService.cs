using Core.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterService
{
    // TODO: Вынести в отдельный класс и юзать там и в других местах
    private const string SAVE_KEY = "CharactersData";
    private const int MAX_CHARACTERS = 5;

    private readonly SaveService _saveService;

    private List<CharacterData> _characters = new();
    private CharacterData _activeCharacter;

    public IReadOnlyList<CharacterData> Characters => _characters;
    public CharacterData ActiveCharacter => _activeCharacter;
    public bool HasCharacters => _characters.Count > 0;

    public event Action<CharacterData> OnCharacterCreated;
    public event Action<CharacterData> OnCharacterUpdated;
    public event Action<string> OnCharacterDeleted;
    public event Action<CharacterData> OnActiveCharacterChanged;

    public CharacterService(SaveService saveService)
    {
        _saveService = saveService;
    }

    public void Initialize()
    {
        Load();
        _activeCharacter = _characters.Find(c => c.IsActive);
        Debug.Log($"Characters: {_characters.Count}");
    }

    public CharacterData CreateCharacter(string name, CharacterClassData classData)
    {
        if (_characters.Count >= MAX_CHARACTERS)
        {
            Debug.LogWarning("Max characters reached");
            return null;
        }

        var character = new CharacterData
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            ClassName = classData.ClassName,
            HP = classData.StartHP,
            Damage = classData.StartDamage
        };

        _characters.Add(character);
        Save();

        OnCharacterCreated?.Invoke(character);
        Debug.Log($"Created: {name} ({classData.ClassName})");

        return character;
    }

    public void UpdateCharacter(CharacterData character)
    {
        var index = _characters.FindIndex(c => c.Id == character.Id);
        if (index < 0) return;

        _characters[index] = character;
        Save();

        OnCharacterUpdated?.Invoke(character);
    }

    public void DeleteCharacter(string characterId)
    {
        var character = _characters.Find(c => c.Id == characterId);
        if (character == null) return;

        if (character.IsActive)
            _activeCharacter = null;

        _characters.Remove(character);
        Save();

        OnCharacterDeleted?.Invoke(characterId);
        Debug.Log($"[CharacterService] Deleted: {characterId}");
    }

    public CharacterData GetCharacter(string characterId)
        => _characters.Find(c => c.Id == characterId);

    public void SetActiveCharacter(CharacterData character)
    {
        if (_activeCharacter != null)
            _activeCharacter.IsActive = false;

        character.IsActive = true;
        _activeCharacter = character;
        Save();

        OnActiveCharacterChanged?.Invoke(character);
    }

    public void AddExperience(CharacterData character, int amount)
    {
        character.Experience += amount;

        while (character.Experience >= character.ExperienceToNextLevel)
        {
            character.Experience -= character.ExperienceToNextLevel;
            LevelUp(character);
        }

        UpdateCharacter(character);
    }

    private void LevelUp(CharacterData character)
    {
        character.Level++;
        character.HP += 10;
        character.Damage += 2;
        Debug.Log($"Level up! {character.Name} → Lv.{character.Level}");
    }

    public void AddKill(CharacterData character)
    {
        character.Kills++;
        UpdateCharacter(character);
    }

    public bool ResetSkillPoints(CharacterData character, CurrencyService currencyService)
    {
        // TODO: ВЫнети в SO
        const int COST = 10000;
        if (!currencyService.SpendGold(COST)) return false;

        character.BonusHP = 0;
        character.BonusDamage = 0;
        character.LifeSteal = 0;
        character.DoublePunch = 0;
        character.CritChance = 0;
        character.CoinBonus = 0;
        UpdateCharacter(character);
        return true;
    }

    public bool ResetClass(CharacterData character, CurrencyService currencyService)
    {
        // TODO: ВЫнети в SO
        const int COST = 50;
        if (!currencyService.SpendDiamonds(COST)) return false;

        UpdateCharacter(character);
        return true;
    }

    public bool Prestige(CharacterData character)
    {
        character.Prestige++;
        character.Level = 1;
        character.Experience = 0;
        UpdateCharacter(character);
        return true;
    }

    private void Save()
    {
        _saveService.Save(SAVE_KEY, new CharactersSaveData { Characters = _characters });
    }

    private void Load()
    {
        var data = _saveService.Load(SAVE_KEY, new CharactersSaveData());
        _characters = data.Characters ?? new List<CharacterData>();
    }
}