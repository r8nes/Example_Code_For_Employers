using UnityEngine;
using Core.Services;
using Core.MVP;

namespace UI.Windows.CharacterInfo
{
    public class CharacterEditPresenter : BasePresenter<CharacterEditWindow>
    {
        private readonly UIService _uiService;
        private readonly CharacterService _characterService;
        private readonly CurrencyService _currencyService;

        private CharacterData _character;
        private CharacterClassData _classData;

        public CharacterEditPresenter(
            UIService uiService,
            CharacterService characterService,
            CurrencyService currencyService)
        {
            _uiService = uiService;
            _characterService = characterService;
            _currencyService = currencyService;
        }

        protected override void OnInitialize()
        {
            _character = _characterService.ActiveCharacter;
            if (_character != null) RefreshView();
        }

        public void ShowCharacter(CharacterData character)
        {
            _character = character;
            if (View != null) RefreshView();
        }

        private void RefreshView()
        {   // TODO: Не забыть перенести в Адресаблз
            _classData = Resources.Load<CharacterClassData>($"{_character.ClassName}");
            View.SetCharacter(_character, _classData);
        }

        public void OnPrestigeClicked()
        {
            if (_character == null) return;
            _characterService.Prestige(_character);
            RefreshView();
            Debug.Log($"Prestige! {_character.Name} > Prestige {_character.Prestige}");
        }

        public void OnResetSkillsClicked()
        {
            if (_character == null) return;
            bool success = _characterService.ResetSkillPoints(_character, _currencyService);
            if (success) RefreshView();
            else Debug.Log("Not enough coinsz");
        }

        public void OnResetClassClicked()
        {
            if (_character == null) return;
            bool success = _characterService.ResetClass(_character, _currencyService);
            if (success) RefreshView();
            else Debug.Log("Not enough diamonds");
        }

        public void OnChangeSkinClicked()
        {
            if (_character == null) return;
            _character.SkinIndex = (_character.SkinIndex + 1) % 5;
            _characterService.UpdateCharacter(_character);
            RefreshView();
        }

        public void OnDeleteClicked()
        {
            if (_character == null) return;

            View.ShowDeleteConfirm(() =>
            {
                _characterService.DeleteCharacter(_character.Id);
                _uiService.Hide<CharacterEditWindow>();
                Debug.Log($"Deleted: {_character.Name}");
            });
        }

        public void OnBackClicked()
        {
            _uiService.Hide<CharacterEditWindow>();
        }
    }
}