using System.Collections.Generic;


namespace WAR.GameEngine
{
    /// <summary>
    /// Класс управления ресурсами города
    /// </summary>
    public class CityResources
    {
        private City owner;
        /// <summary>
        /// Деньги города
        /// </summary>
        //public float MoneyIncome { set; get; } = 0f;

        public ResourceChangeFactors<CityMoneyIncrementFactors> CityMoneyIncFactors = ResourceChangeFactors<CityMoneyIncrementFactors>.CreateInstance();
        public ResourceChangeFactors<CityMoneyDecrementFactors> CityMoneyDecFactors = ResourceChangeFactors<CityMoneyDecrementFactors>.CreateInstance();
        public float MoneyProfit { get { return CityMoneyIncFactors.Sum - CityMoneyDecFactors.Sum; }}

        //строительные материалы
        private float _material = 0f;
        /// <summary>
        /// Строительные материалы города
        /// </summary>
        public float Materials { set { _material = value; } get { return _material; } }

        private float _materialIncome = 0f;
        /// <summary>
        /// Прирост строительных материалов в ход
        /// </summary>
        public float MaterialsIncome { set { _materialIncome = value; } get { return _materialIncome; } }

        //еда
        private float _food = 0f;
        /// <summary>
        /// Пища
        /// </summary>
        public float Food { set { _food = value; } get { return _food; } }

        //private float _foodIncrement =0f; // не нужно, т.к. есть FoodIncrementFactors.Sum
        /// <summary>
        /// Прирост пищи в ход
        /// </summary>
        //public float FoodIncrement { set { _foodIncrement = value; } get { return _foodIncrement; } }

        /// <summary>
        /// Пока добавил в тестовом режиме 
        /// На приток еды влияют - базовое здание,  микрорайоны,спецтайлы (содержимое enum-а FoodIncrementFactors)
        /// </summary>
        public ResourceChangeFactors<FoodIncrementFactors> FoodIncFactors = ResourceChangeFactors<FoodIncrementFactors>.CreateInstance();


        //private float _foodDecrement = 0f;
        /// <summary>
        /// величина расхода пищи в ход по модулю
        /// </summary>
        //public float FoodDecrement { set { _foodDecrement = UnityEngine.Mathf.Abs(value); } get { return _foodDecrement; } }

        public ResourceChangeFactors<FoodDecrementFactors> FoodDecFactors = ResourceChangeFactors<FoodDecrementFactors>.CreateInstance();


        /// <summary>
        /// Разность прироста пищи с расходом пищи
        /// </summary>
        public float FoodProfit { get { return (FoodIncFactors.Sum/*_foodIncrement*/ - FoodDecFactors.Sum/*_foodDecrement*/); } }

        private float _population = 0f;
        /// <summary>
        /// Население города
        /// </summary>
        public float Population { set { _population = value; } get { return _population; } }

        //бонусные
        private float _herbs = 0f;
        /// <summary>
        /// Травы
        /// </summary>
        public float Herbs { set { _herbs = value; } get { return _herbs; } }

        private float _cattle = 0f;
        /// <summary>
        /// Скот
        /// </summary>
        public float Cattle { set { _cattle = value; } get { return _cattle; } }

        private float _soil = 0f;
        /// <summary>
        /// Плодородная почва
        /// </summary>
        public float Soil { set { _soil = value; } get { return _soil; } }

        private float _gold = 0f;
        /// <summary>
        /// Золото
        /// </summary>
        public float Gold { set { _gold = value; } get { return _gold; } }

        private float _silver = 0f;
        /// <summary>
        /// Серебро
        /// </summary>
        public float Silver { set { _silver = value; } get { return _silver; } }

        private float _copper = 0f;
        /// <summary>
        /// Медь
        /// </summary>
        public float Copper { set { _copper = value; } get { return _copper; } }

        /// <summary>
        /// Конструктор
        /// </summary>   
        public CityResources(City city)
        {               
            Messenger.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_STEP, OnNextStep);
            owner = city;
        }
        
        /// <summary>
        /// Деструктор
        /// </summary>
        ~CityResources()
        {
            Messenger.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_STEP, OnNextStep);
        }

        /// <summary>
        /// Обработка конца хода
        /// </summary>
        void OnNextStep()
        {
            //_material += _materialIncome;
            //_material = _material < 0 ? 0 : _material;

            //_food += _foodIncome + 0.5f*_cattle + 0.2f*_herbs + 0.8f*_soil;

            //отправляем деньги фракции
            //Messenger<int, float>.Broadcast(GameEvents.GameEngineEvent.ResourcesEvent.MONEY_RESOURCE_CHANGED, owner.ID, CityMoneyIncFactors.Sum/*MoneyIncome*/);
            

            //расчет роста населения города (тестовая формула)
            float foodRation = 0.2f; //коэффициент потребления 1 человеком

            int districtCounts = 0;
            foreach (int _count in owner.DistrictCounts)
            {
                districtCounts += _count;
            }

            FoodDecFactors.Set(FoodDecrementFactors.population, _population * foodRation);/*_foodDecrement*/
            //FoodDecrement = _population * foodRation; 
            float bornMult = 0.1f * districtCounts; //коэффициент рождаемости
            float populationChange = FoodProfit * bornMult;

            //TODO: посмотреть, мб после ввода FoodDecrement этот блок можно упростить
            //если еды не хватает, но есть запас
            if (populationChange < 0 && _food > 0)
            {
                //если запаса не хватает
                if (_food < System.Math.Abs(FoodProfit))
                {
                    populationChange = (FoodIncFactors.Sum/*_foodIncrement*/ + _food - FoodDecFactors.Sum/*_foodDecrement*/) * bornMult;
                    _food = 0f;
                }
                else //если запаса хватает 
                {
                    populationChange = 0f;
                    _food = _food - System.Math.Abs(FoodProfit);
                }
            }

            _population += populationChange;

            //проверка на максимум населения
            int maxPopulation = owner.GetDistrictCount(1) * 75 + owner.GetDistrictCount(5) * 350 + owner.GetDistrictCount(2) * 200 + owner.GetDistrictCount(3) * 300 + owner.GetDistrictCount(4) * 250;

            if (_population > maxPopulation)
                _population = maxPopulation;

        }
    }
}
