using Core.Services;
using System.Collections.Generic;
using UI.Windows.CharacterCreate;
using UnityEngine;

public class CharacterCreatePresenter : Core.MVP.BasePresenter<CharacterCreateWindow>
{
    private readonly UIService _uiService;
    private readonly CharacterService _characterService;

    private List<CharacterClassData> _classes = new();
    private int _currentClassIndex = 0;
    private bool _hardcoreMode = false;
    private string _currentName = "";

    public CharacterCreatePresenter(UIService uiService, CharacterService characterService)
    {
        _uiService = uiService;
        _characterService = characterService;
    }

    protected override void OnInitialize()
    {
        LoadClasses();
        ShowCurrentClass();
    }

    private void LoadClasses()
    {
        // TODO: Не забыть перенести в Адресаблз
        var loaded = Resources.LoadAll<CharacterClassData>("Characters");
        _classes = new List<CharacterClassData>(loaded);

        if (_classes.Count == 0)
        {
            Debug.LogWarning("No CharacterClassData");
        }

        Debug.Log($"Loaded {_classes.Count}");
    }

    private void ShowCurrentClass()
    {
        if (_classes.Count == 0) return;

        var classData = _classes[_currentClassIndex];
        View.SetClassData(classData);
        View.SetNavigationButtons(
            hasPrev: _currentClassIndex > 0,
            hasNext: _currentClassIndex < _classes.Count - 1
        );
    }
    public void OnPrevClassClicked()
    {
        if (_currentClassIndex > 0)
        {
            _currentClassIndex--;
            ShowCurrentClass();
        }
    }

    public void OnNextClassClicked()
    {
        if (_currentClassIndex < _classes.Count - 1)
        {
            _currentClassIndex++;
            ShowCurrentClass();
        }
    }

    public void OnHardcoreToggled()
    {
        _hardcoreMode = !_hardcoreMode;
        View.SetHardcoreMode(_hardcoreMode);
    }

    public void OnNameChanged(string name)
    {
        _currentName = name.Trim();
        View.SetCreateButtonActive(_currentName.Length >= 2);
    }

    public void OnCreateClicked(string name)
    {
        name = name.Trim();
        if (name.Length < 2) return;

        var classData = _classes[_currentClassIndex];
        var character = _characterService.CreateCharacter(name, classData);

        if (character == null) return;

        character.HardcoreMode = _hardcoreMode;
        _characterService.SetActiveCharacter(character);

        View.ShowCreatedAnimation(() =>
        {
            Debug.Log($"Created: {name} ({classData.ClassName})");
            _uiService.Hide<CharacterCreateWindow>();
            _uiService.Show<UI.Windows.MainMenu.MainMenuWindow>();
        });
    }

    public void OnBackClicked()
    {
        _uiService.Hide<CharacterCreateWindow>();
    }
}