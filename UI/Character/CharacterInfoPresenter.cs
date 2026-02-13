using Core.MVP;
using Core.Services;
using UI.Windows.CharacterInfo;
using UnityEngine;

public class CharacterInfoPresenter : BasePresenter<CharacterInfoWindow>
{
    private readonly UIService _uiService;
    private readonly CharacterService _characterService;

    private CharacterData _character;

    public CharacterInfoPresenter(UIService uiService, CharacterService characterService)
    {
        _uiService = uiService;
        _characterService = characterService;
    }

    protected override void OnInitialize()
    {
        _character = _characterService.ActiveCharacter;
        if (_character != null)
        {
            View.SetCharacter(_character);
        }
    }

    public void ShowCharacter(CharacterData character)
    {
        _character = character;
        _characterService.SetActiveCharacter(character);
        View.SetCharacter(character);
    }

    public void OnAdventureClicked()
    {
        if (_character == null) return;
        Debug.Log($"Starting {_character.Name}");
        _uiService.Hide<CharacterInfoWindow>();
    }

    public void OnShortcutClicked()
    {
    }

    public void OnEditClicked()
    {
        _uiService.Hide<CharacterInfoWindow>();
        var editWindow = _uiService.GetWindow<CharacterEditWindow>();

        editWindow.Presenter?.ShowCharacter(_character);
        _uiService.Show<CharacterEditWindow>();
    }

    public void OnBackClicked()
    {
        _uiService.Hide<CharacterInfoWindow>();
    }
}