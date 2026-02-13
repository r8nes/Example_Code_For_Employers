using Core.DI;
using Core.Services;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.CharacterCreate
{
    public class CharacterCreateWindow : Window, IConstructable<CharacterCreatePresenter>
    {
        [Header("Class Selection")]
        [SerializeField] private Button _prevClassButton;
        [SerializeField] private Button _nextClassButton;
        [SerializeField] private TextMeshProUGUI _classNameText;
        [SerializeField] private Image _characterSprite;

        [Header("Stats Panel")]
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _damageBar;
        [SerializeField] private Slider _difficultyBar;

        [Header("Class Info")]
        [SerializeField] private TextMeshProUGUI _levelSkillsText;
        [SerializeField] private TextMeshProUGUI _weaknessText;

        [Header("Hardcore Toggle")]
        [SerializeField] private Button _hardcoreButton;
        [SerializeField] private TextMeshProUGUI _hardcoreButtonText;

        [Header("Name Input")]
        [SerializeField] private TMP_InputField _nameInputField;

        [Header("Buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _backButton;

        public override WindowType WindowType => WindowType.Screen;

        private CharacterCreatePresenter _presenter;

        public void Construct(CharacterCreatePresenter presenter)
        {
            _presenter = presenter;
            _presenter.SetView(this);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _prevClassButton.onClick.AddListener(() => _presenter?.OnPrevClassClicked());
            _nextClassButton.onClick.AddListener(() => _presenter?.OnNextClassClicked());
            _hardcoreButton.onClick.AddListener(() => _presenter?.OnHardcoreToggled());
            _backButton.onClick.AddListener(() => _presenter?.OnBackClicked());

            _createButton.interactable = false;
            _createButton.onClick.AddListener(OnCreateButtonClicked);

            if (_nameInputField != null)
                _nameInputField.onValueChanged.AddListener((text) => _presenter?.OnNameChanged(text));
            else
                Debug.LogError("Name is NULL!");

            _presenter?.Initialize();
        }

        private void OnCreateButtonClicked()
        {
            var name = _nameInputField != null ? _nameInputField.text : "";
            _presenter?.OnCreateClicked(name);
        }

        protected override void OnCleanup()
        {
            base.OnCleanup();
            _prevClassButton.onClick.RemoveAllListeners();
            _nextClassButton.onClick.RemoveAllListeners();
            _hardcoreButton.onClick.RemoveAllListeners();
            _createButton.onClick.RemoveListener(OnCreateButtonClicked);
            _backButton.onClick.RemoveAllListeners();
            _nameInputField.onValueChanged.RemoveAllListeners();
            _presenter?.Dispose();
        }

        public void SetClassData(CharacterClassData classData)
        {
            _classNameText.DOFade(0f, 0.1f).OnComplete(() =>
            {
                _classNameText.text = $"- {classData.ClassName} -";
                _classNameText.DOFade(1f, 0.15f);
            });

            if (classData.ClassSprite != null)
            {
                _characterSprite.sprite = classData.ClassSprite;
                _characterSprite.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
            }

            _healthBar.value = classData.BaseHealth;
            _damageBar.value = classData.BaseDamage;
            _difficultyBar.value = classData.BaseDifficulty;

            _levelSkillsText.text = "Уровневые Умения:\n" +
                string.Join("\n", classData.LevelSkills);

            _weaknessText.text = string.IsNullOrEmpty(classData.Weakness)
                ? ""
                : $"Слабость:\n{classData.Weakness}";
        }

        public void SetHardcoreMode(bool isOn)
        {
            _hardcoreButtonText.text = isOn ? "Выключить Хардкор" : "Включить Хардкор";

            var color = isOn
                ? new Color(0.8f, 0.2f, 0.2f, 1f)
                : new Color(0.3f, 0.3f, 0.3f, 1f);

            _hardcoreButton.GetComponent<Image>().DOColor(color, 0.2f);
        }

        public void SetCreateButtonActive(bool active)
        {
            _createButton.interactable = active;
        }

        public void SetNavigationButtons(bool hasPrev, bool hasNext)
        {
            _prevClassButton.interactable = hasPrev;
            _nextClassButton.interactable = hasNext;
        }

        public void ShowCreatedAnimation(System.Action onComplete)
        {
            _createButton.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}