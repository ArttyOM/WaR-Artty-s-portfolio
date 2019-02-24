using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using WAR.GameEngine;

namespace WAR
{

    /// <summary>
    /// Обновляет локальные (городские) ресурсы на соответствующей плашке в CityView,
    /// например: Еда, деньги, население, материалы
    /// </summary>
    public class CityResourcesService : MonoBehaviour
    {

        private City currentSelectedCity;
        [SerializeField] private Transform _CityResourcesPivot;

        [SerializeField] private Text _cityMoneyProfitChangedText;

        [SerializeField] private Transform _CityProfitInfoPivot;
        private GameObject _CityProfitInfoGObj;
        private Text _cityResourceIncrement;
        private Text _cityResourceDecrement;

        private Dictionary<GameEvents1.CityResourcesChanged, Text> _resourceList = new Dictionary<GameEvents1.CityResourcesChanged, Text>();
        private Dictionary<GameEvents1.CityResourcesProfitChanged, Text> _resourceProfitList = new Dictionary<GameEvents1.CityResourcesProfitChanged, Text>();

        string[] cityResourcesChangeEvents = Enum.GetNames(typeof(GameEvents1.CityResourcesChanged));
        string[] cityResourcesProfitChangeEvents = Enum.GetNames(typeof(GameEvents1.CityResourcesProfitChanged));

        /// <summary>
        ///Onvalidate, Awake, Start тут,
        ///OnDeastroy тоже
        /// </summary>
        #region Предустановки

        ///автопарсинг прямо из сцены, чтобы мозги не парить при дальнейшей разработке
        private void OnValidate()
        {
            if (_CityResourcesPivot == null)
            {
                Debug.Log("В CityResourcesService не указана ссылка на UI-контейнер с ресурсами");
                return;
            }

            if (_CityProfitInfoPivot == null)
            {
                Debug.Log("В CityResourcesService не указана ссылка на всплывающее окно с подробной инфой к ресурсам");
            }
            else
            {
                _cityResourceIncrement = _CityProfitInfoPivot.GetChild(0).GetComponent<Text>();
                _cityResourceDecrement = _CityProfitInfoPivot.GetChild(1).GetComponent<Text>();
                _CityProfitInfoGObj = _CityProfitInfoPivot.gameObject;
            }

            Transform tmp, tmp2; //tmp2 - нулевой GameObject с родителем tmp - должен содержать Event для прироста ресурса 
            GameEvents1.CityResourcesChanged parsedEnumValue;
            GameEvents1.CityResourcesProfitChanged parsedEnumValue2;
            for (int i = 0; i < _CityResourcesPivot.childCount; i++)
            {
                //т.к. иерархия имеет вид pivot / child(id)[картинка+наименование] / child(0)[значение ресурса]
                //добавляем в список ресурсов всех потомков потомков
                tmp = _CityResourcesPivot.GetChild(i);

                //пытаемся имя объекта преобразовать к перечислителю CityResources
                if (Enum.TryParse<GameEvents1.CityResourcesChanged>(tmp.name, out parsedEnumValue)) //Аве 4+ .Net
                {
                    //Если получается, добавляем в справочник
                    _resourceList.Add(
                        parsedEnumValue,
                        tmp.GetChild(1).GetComponent<Text>());

                    tmp2 = tmp.GetChild(0);
                    if (Enum.TryParse<GameEvents1.CityResourcesProfitChanged>(tmp2.name, out parsedEnumValue2))
                    {
                        _resourceProfitList.Add(
                            parsedEnumValue2,
                            tmp2.GetComponent<Text>());
                    }

                    //такие простые, но такие страшные и громоздкие операции. УУУУ
                }
            }
            //отдельно добавляем прирост денег города (без танцев с бубнами)
            _resourceProfitList.Add(GameEvents1.CityResourcesProfitChanged.CITY_MONEY_PROFIT_CHANGED, _cityMoneyProfitChangedText);
        }

        private void Awake()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < cityResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Trying to add Listener [" + cityResourcesChangeEvents[i] + "]");
                //handler = UpdateResource; //UpdateResource(int cityID, float value, string Event)
                Messenger<int, float>.AddListener(cityResourcesChangeEvents[i], UpdateResource);
            }

            for (int i = 0; i < cityResourcesProfitChangeEvents.Length; i++)
            {
                Messenger<int, float>.AddListener(cityResourcesProfitChangeEvents[i], UpdateProfit);
            }

            Messenger<City>.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, UpdateAllResources);

            if (_CityProfitInfoGObj)_CityProfitInfoGObj.SetActive(false);
        }

        private void OnDestroy()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < cityResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Listener's number " + i);
                //handler = UpdateResource;
                Messenger<int, float>.RemoveListener(
                    cityResourcesChangeEvents[i], UpdateResource
                    ); //вызываем обобщенно, потом уже смотрим на строку ивента
            }

            for (int i = 0; i < cityResourcesProfitChangeEvents.Length; i++)
            {
                Messenger<int, float>.RemoveListener(cityResourcesProfitChangeEvents[i], UpdateProfit);
            }

            Messenger<City>.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, UpdateAllResources);
        }
        #endregion Предустановки


        /// </summary>
        /// <param name="cityResourceIndex"></param>
        /// <param name="cityID"></param>
        /// <param name="value"></param>
        private void UpdateResource(int cityID, float value, string MyEvent)
        {
            //Debug.Log("cityID " + cityID + " value " + value + " MyEvent " + MyEvent);
            if  ((currentSelectedCity==null)||(currentSelectedCity.ID != cityID)) return;
            //если бот/другой игрок вызвал для своего другого города апдейт ресурсов, наш выбранный сейчас город это не колышет

            Text textGUI;
            GameEvents1.CityResourcesChanged enumValue = (GameEvents1.CityResourcesChanged)Enum.Parse(typeof(GameEvents1.CityResourcesChanged), MyEvent);

            
            if (_resourceList.TryGetValue(enumValue, out textGUI))
            {
                float tmp = (int)(10f * value) / 10f;
                textGUI.text = tmp.ToString();
            }
            else Debug.Log("Для ресурса с ивентом " + MyEvent + " не существует визуального отображения на CityView");
        }

        /// <summary>
        /// отображаем изменения профита
        /// </summary>
        /// <param name="cityID"></param>
        /// <param name="value"></param>
        /// <param name="MyEvent"></param>
        private void UpdateProfit(int cityID, float value, string MyEvent)
        {
            //Debug.Log("cityID " + cityID + " value " + value + " MyEvent " + MyEvent);
            if ((currentSelectedCity == null) || (currentSelectedCity.ID != cityID)) return;
            //если бот/другой игрок вызвал для своего другого города апдейт ресурсов, наш выбранный сейчас город это не колышет

            Text textGUI;
            GameEvents1.CityResourcesProfitChanged enumValue = (GameEvents1.CityResourcesProfitChanged)Enum.Parse(typeof(GameEvents1.CityResourcesProfitChanged), MyEvent);

            if (_resourceProfitList.TryGetValue(enumValue, out textGUI))
            {
                float tmp = (int)(10f * value) / 10f;

                //Debug.Log("СМОТРИ СЮДЫ" + ((float)((int)10 * value)));
                //Debug.Log("СМОТРИ СЮДЫ" + (int)(10f * value) / 10f);
                //Debug.Log(textGUI.gameObject.name);
                if (value < 0f)
                {
                    textGUI.text = tmp.ToString();
                }
                else
                {
                    textGUI.text = "+" + tmp.ToString();
                }//ключевое отличие от UpdateResource
            }
            else Debug.Log("Для ресурса с ивентом " + MyEvent + " не существует визуального отображения на CityView");
        }


        private void UpdateAllResources(City city)
        {
            //Debug.Log("UpdateAllResources вызван");
            //Debug.Log("Food= " + city.CityResources.Food + ", FoodIncome=" + city.CityResources.FoodProfit);

            currentSelectedCity = city;

            //TODO Artty заставить Виталю переделать CityResources из кучи float-переменных в один Dictionary.
            UpdateResource(city.ID, city.CityResources.Food, GameEvents1.CityResourcesChanged.FOOD_RESOURCE_CHANGED.ToString());
            //UpdateResource(city.ID, city.CityResources.MaterialsIncome, GameEvents1.CityResourcesChanged.MATERIALS_RESOURCE_CHANGED.ToString());
            //UpdateResource(city.ID, city.CityResources.MoneyIncome, GameEvents1.CityResourcesChanged.MONEY_INCOME_RESOURCE_CHANGED.ToString());
            //UpdateResource(city.ID, city.CityResources.Population, GameEvents1.CityResourcesChanged.POPULATION_RESOURCE_CHANGED.ToString());

            UpdateProfit(city.ID, city.CityResources.FoodProfit, GameEvents1.CityResourcesProfitChanged.FOOD_PROFIT_CHANGED.ToString());
            UpdateProfit(city.ID, city.CityResources.MoneyProfit, GameEvents1.CityResourcesProfitChanged.CITY_MONEY_PROFIT_CHANGED.ToString());

        }


        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(1))
        //    { _CityProfitInfoGObj.SetActive(true); }
        //    if (Input.GetMouseButton(1))
        //    {
        //        var tmp = Input.mousePosition;
        //        tmp.x += 130f;
        //        tmp.y += 50f;
        //        tmp.z = 0;
        //        _CityProfitInfoPivot.position = tmp;
        //    }
        //    if (Input.GetMouseButtonUp(1))
        //    { _CityProfitInfoGObj.SetActive(false); }
        //}

        #region невостребованный код
        /// <summary>
        /// Метод отображает в GUI float-значения  ресурсов согласно поданному на вход "справочнику" Resourses 
        /// string-ключ содержит наименование ресурса
        /// float - его количественное значение
        /// </summary>
        /// <param name="Resourses"></param>
        //private void UpdateResources(Dictionary<CityResources, float> Resources)
        //{
        //    //Debug.Log("сообщение получено");
        //    //Debug.Log("полученный справочник:");
        //    //foreach (KeyValuePair<string, float> resource in Resources)
        //    //{
        //    //    Debug.Log(resource.Key + " " + resource.Value);
        //    //}

        //    //пересчет ресурсов будет происходить вне GUI-системы
        //    //в GUI же просто считываем данные по ресурсам для их отображения
        //    Text textGUI;
        //    //ищем совпадения ключей внешнего справочника со списком ресурсов GUI
        //    foreach (KeyValuePair<CityResources, float> resource in Resources)
        //    {
        //        _resourceList.TryGetValue(resource.Key, out textGUI);
        //        textGUI.text = resource.Value.ToString(); //как сильны мои лапища, раньше это занимало 15 строк кода
        //    }
        //}
        #endregion невостребованный код
    }
}