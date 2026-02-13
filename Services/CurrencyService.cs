using System;
using UnityEngine;

namespace Core.Services
{
    public class CurrencyService
    {
        // TODO: Вынести в отдельный класс и юзать там и в других местах
        private const string SAVE_KEY = "CurrencyData";

        private readonly SaveService _saveService;

        private int _gold;
        private int _diamonds;

        public int Gold => _gold;
        public int Diamonds => _diamonds;

        public event Action<int, int> OnCurrencyChanged;

        public CurrencyService(SaveService saveService)
        {
            _saveService = saveService;
        }

        public void Initialize()
        {
            var saved = _saveService.Load(SAVE_KEY, new CurrencyData
            {
                Gold = 0,
                Diamonds = 0
            });

            _gold = saved.Gold;
            _diamonds = saved.Diamonds;

            Debug.Log($"Initialized — Gold: {_gold}, Diamonds: {_diamonds}");
        }

        public void AddGold(int amount)
        {
            _gold += amount;
            NotifyAndSave();
        }

        public void AddDiamonds(int amount)
        {
            _diamonds += amount;
            NotifyAndSave();
        }

        public bool SpendGold(int amount)
        {
            if (_gold < amount) return false;
            _gold -= amount;
            NotifyAndSave();
            return true;
        }

        public bool SpendDiamonds(int amount)
        {
            if (_diamonds < amount) return false;
            _diamonds -= amount;
            NotifyAndSave();
            return true;
        }

        private void NotifyAndSave()
        {
            OnCurrencyChanged?.Invoke(_gold, _diamonds);
            _saveService.Save(SAVE_KEY, new CurrencyData
            {
                Gold = _gold,
                Diamonds = _diamonds
            });
        }
    }
}