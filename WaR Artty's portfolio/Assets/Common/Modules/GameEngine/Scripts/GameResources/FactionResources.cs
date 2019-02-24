using System.Collections.Generic;

namespace WAR.GameEngine
{
    /// <summary>
    /// Класс упраления ресурсами фракции
    /// </summary>
    public class FactionResources
    {
        //деньги, деньги, дребеденьги
        private float _money = 0f;
        /// <summary>
        /// Деньги
        /// </summary>
        public float Money { set { _money = value; _money = _money < 0 ? 0 : _money; } get { return _money; } }

        public ResourceChangeFactors<FactionMoneyIncrementFactors> FactionMoneyIncFactors = ResourceChangeFactors<FactionMoneyIncrementFactors>.CreateInstance();
        public ResourceChangeFactors<FactionMoneyDecrementFactors> FactionMoneyDecFactors = ResourceChangeFactors<FactionMoneyDecrementFactors>.CreateInstance();

        public float MoneyProfit { get { return FactionMoneyIncFactors.Sum - FactionMoneyDecFactors.Sum; } }

        //tier 1 (общие)
        private float _wood = 0f;
        /// <summary>
        /// Дерево
        /// </summary>
        public float Wood { set { _wood = value; } get { return _wood; } }

        private float _woodIncome = 0f;
        /// <summary>
        /// Прирост дерева в ход
        /// </summary>
        public float WoodIncome { set { _woodIncome = value; } get { return _woodIncome; } }

        private float _stone = 0f;
        /// <summary>
        /// Камень
        /// </summary>
        public float Stone { set { _stone = value; } get { return _stone; } }

        private float _stoneIncome = 0f;
        /// <summary>
        /// Прирост камня в ход
        /// </summary>
        public float StoneIncome { set { _stoneIncome = value; } get { return _stoneIncome; } }

        private float _iron = 0f;
        /// <summary>
        /// Железо
        /// </summary>
        public float Iron { set { _iron = value; } get { return _iron; } }

        private float _ironIncome = 0f;
        /// <summary>
        /// Прирост железа в ход
        /// </summary>
        public float IronIncome { set { _ironIncome = value; } get { return _ironIncome; } }

        private float _hide = 0f;
        /// <summary>
        /// Шкуры
        /// </summary>
        public float Hide { set { _hide = value; } get { return _hide; } }        

        private float _hideIncome = 0f;
        /// <summary>
        /// Прирост шкур в ход
        /// </summary>
        public float HideIncome { set { _hideIncome = value; } get { return _hideIncome; } }

        private float _horse = 0f;
        /// <summary>
        /// Лошади
        /// </summary>
        public float Horse { set { _horse = value; } get { return _horse; } }

        private float _horseIncome = 0f;
        /// <summary>
        /// Прирост лошадей в ход
        /// </summary>
        public float HorseIncome { set { _horseIncome = value; } get { return _horseIncome; } }

        //tier 2 (фракционные смешаные || -1 - не доступен ресурс)
        private float _manaStone = -1f;
        /// <summary>
        /// Камень маны
        /// </summary>
        public float ManaStone { set { _manaStone = value; } get { return _manaStone; } }

        private float _manaStoneIncome = 0f;
        /// <summary>
        /// Пирост камней маны
        /// </summary>
        public float ManaStoneIncome { set { _manaStoneIncome = value; } get { return _manaStoneIncome; } }

        private float _mithril = -1f;
        /// <summary>
        /// Мифрил
        /// </summary>
        public float Mithril { set { _mithril = value; } get { return _mithril; } }

        private float _mithrilIncome = 0f;
        /// <summary>
        /// Прирост мифрила в ход
        /// </summary>
        public float MithrilIncome { set { _mithrilIncome = value; } get { return _mithrilIncome; } }

        private float _charcoal = -1f;
        /// <summary>
        /// Уголь
        /// </summary>
        public float Charcoal { set { _charcoal = value; } get { return _charcoal; } }

        private float _charcoalIncome = 0f;
        /// <summary>
        /// Прирост угля в ход
        /// </summary>
        public float CharcoalIncome { set { _charcoalIncome = value; } get { return _charcoalIncome; } }

        //tier 3 (фракционный || -1 - не доступен ресурс)
        private float _factionResource = -1f;
        /// <summary>
        /// Фракционный ресурс
        /// </summary>
        public float FactionResource { set { _factionResource = value; } get { return _factionResource; } }

        private float _factionResourceIncome = 0f;
        /// <summary>
        /// Прирост фракционного ресурса в ход
        /// </summary>
        public float FactionResourceIncome { set { _factionResourceIncome = value; } get { return _factionResourceIncome; } }

        private float _cattleSummary = 0f;
        /// <summary>
        /// Скот со всех городов (для учета добавки к шкурам)
        /// </summary>
        public float CattleSummary { set { _cattleSummary = value; } get { return _cattleSummary; } }

        private float _populationSummary = 0f;
        /// <summary>
        /// Население фракции
        /// </summary>
        public float PopulationSummary { set { _populationSummary = value; } get { return _populationSummary; } }

        /// <summary>
        /// Конструктор
        /// </summary>   
        public FactionResources()
        {
            Messenger.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_STEP, OnNextStep);
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~FactionResources()
        {
            Messenger.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_STEP, OnNextStep);
        }

        /// <summary>
        /// Обработка конца хода
        /// </summary>
        void OnNextStep()
        { 
            _wood += _woodIncome;
            _wood = _wood < 0 ? 0 : _wood;

            _stone += _stoneIncome;
            _stone = _stone < 0 ? 0 : _stone;

            _iron += _ironIncome;
            _iron = _iron < 0 ? 0 : _iron;

            _hide += _hideIncome + 0.2f*_cattleSummary;
            _hide = _hide < 0 ? 0 : _hide;

            _horse += _horseIncome;
            _horse = _horse < 0 ? 0 : _horse;

            if (_manaStone >= 0)
            {
                _manaStone += _manaStoneIncome;
                _manaStone = _manaStone < 0 ? 0 : _manaStone;
            }

            if (_mithril >= 0)
            {
                _mithril += _mithrilIncome;
                _mithril = _mithril < 0 ? 0 : _mithril;
            }

            if (_charcoal >= 0)
            {
                _charcoal += _charcoalIncome;
                _charcoal = _charcoal < 0 ? 0 : _charcoal;
            }

            if (_factionResource >= 0)
            {
                _factionResource += _factionResourceIncome;
                _factionResource = _factionResource < 0 ? 0 : _factionResource;
            }

            Money += MoneyProfit;
           
        }

    }
}
