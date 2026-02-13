using Core.DI;
using Core.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : Window, IConstructable<CharacterInfoPresenter>
{
    [Header("Character Preview")]
    [SerializeField] private Image _characterSprite;
    [SerializeField] private TextMeshProUGUI _characterNameText;
    [SerializeField] private TextMeshProUGUI _classNameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _prestigeText;

    [Header("Buttons")]
    [SerializeField] private Button _adventureButton; 
    [SerializeField] private Button _shortcutButton; 
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _backButton; 

    [Header("Button Text")]
    [SerializeField] private TextMeshProUGUI _adventureButtonText;

    public override WindowType WindowType => WindowType.Overlay;

    private CharacterInfoPresenter _presenter;
    public CharacterInfoPresenter Presenter => _presenter;

    public void Construct(CharacterInfoPresenter presenter)
    {
        _presenter = presenter;
        _presenter.SetView(this);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _adventureButton.onClick.AddListener(() => _presenter?.OnAdventureClicked());
        _shortcutButton.onClick.AddListener(() => _presenter?.OnShortcutClicked());
        _editButton.onClick.AddListener(() => _presenter?.OnEditClicked());
        _backButton.onClick.AddListener(() => _presenter?.OnBackClicked());

        _shortcutButton.interactable = false;
        _presenter?.Initialize();
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        _adventureButton.onClick.RemoveAllListeners();
        _shortcutButton.onClick.RemoveAllListeners();
        _editButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        _presenter?.Dispose();
    }

    public void SetCharacter(CharacterData character)
    {
        _characterNameText.text = character.Name;
        _classNameText.text = $"- {character.ClassName} -";
        _levelText.text = $"Ур. {character.Level}";
        _killsText.text = $"Убийства: {character.Kills}";
        _prestigeText.text = character.Prestige > 0 ? $"Престиж {character.Prestige}" : "";
        _adventureButtonText.text = character.GamesPlayed > 0 ? "Продолжить" : "Начать";

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
}