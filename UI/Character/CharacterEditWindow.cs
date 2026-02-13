using UnityEngine;
using UnityEngine.UI;
using Core.Services;
using Core.DI;
using DG.Tweening;
using TMPro;

namespace UI.Windows.CharacterInfo
{
    public class CharacterEditWindow : Window, IConstructable<CharacterEditPresenter>
    {
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private TextMeshProUGUI _classNameText;

        [Header("Character Preview")]
        [SerializeField] private Image _characterSprite;
        [SerializeField] private TextMeshProUGUI _killsText;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _lifeStealText;
        [SerializeField] private TextMeshProUGUI _doublePunchText;

        [Header("Level Skills")]
        [SerializeField] private Transform _skillsContainer;
        [SerializeField] private TextMeshProUGUI _skillsListText;

        [Header("Prestige")]
        [SerializeField] private Button _prestigeButton;
        [SerializeField] private TextMeshProUGUI _prestigeLabel;

        [Header("Reset Buttons")]
        [SerializeField] private Button _resetSkillsButton;
        [SerializeField] private Button _resetClassButton;
        [SerializeField] private Button _changeSkinButton;

        [Header("Action Buttons")]
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _backButton;

        public override WindowType WindowType => WindowType.Overlay;

        private CharacterEditPresenter _presenter;

        public CharacterEditPresenter Presenter => _presenter;

        public void Construct(CharacterEditPresenter presenter)
        {
            _presenter = presenter;
            _presenter.SetView(this);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _prestigeButton.onClick.AddListener(() => _presenter?.OnPrestigeClicked());
            _resetSkillsButton.onClick.AddListener(() => _presenter?.OnResetSkillsClicked());
            _resetClassButton.onClick.AddListener(() => _presenter?.OnResetClassClicked());
            _changeSkinButton.onClick.AddListener(() => _presenter?.OnChangeSkinClicked());
            _deleteButton.onClick.AddListener(() => _presenter?.OnDeleteClicked());
            _backButton.onClick.AddListener(() => _presenter?.OnBackClicked());
            _presenter?.Initialize();
        }

        protected override void OnCleanup()
        {
            base.OnCleanup();
            _prestigeButton.onClick.RemoveAllListeners();
            _resetSkillsButton.onClick.RemoveAllListeners();
            _resetClassButton.onClick.RemoveAllListeners();
            _changeSkinButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _presenter?.Dispose();
        }

        public void SetCharacter(CharacterData character, CharacterClassData classData)
        {
            // TODO: Локализация
            _characterNameText.text = character.Name;
            _classNameText.text = $"- {character.ClassName} -";
            _killsText.text = $"Убийства:\n{character.Kills}";

            _levelText.text = $"Ур. {character.Level}";
            _hpText.text = $"ОЗ: {character.TotalHP} [+{character.BonusHP}]";
            _damageText.text = $"Урон: {character.TotalDamage} [+{character.BonusDamage}]";
            _lifeStealText.text = $"Похищение жизни: {character.LifeSteal:0.0}%";
            _doublePunchText.text = $"Двойной удар: {character.DoublePunch:0.0}%";

            if (classData != null && _skillsListText != null)
            {
                _skillsListText.text = "Уровневые Умения:\n" +
                    string.Join("\n", classData.LevelSkills);
            }

            if (_prestigeLabel != null)
            {
                _prestigeLabel.text = character.Prestige > 0
                    ? $"Престиж {character.Prestige}"
                    : "Престиж";
            }

            if (classData?.ClassSprite != null)
            {
                _characterSprite.sprite = classData.ClassSprite;
            }
        }

        public void ShowDeleteConfirm(System.Action onConfirm)
        {
            _deleteButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f)
                .OnComplete(() => onConfirm?.Invoke());
        }
    }
}