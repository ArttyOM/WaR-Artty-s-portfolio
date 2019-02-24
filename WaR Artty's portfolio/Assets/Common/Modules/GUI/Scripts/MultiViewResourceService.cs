using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using WAR.GameEngine;

namespace WAR
{
    public class MultiViewResourceService : MonoBehaviour
    {
        private Faction _faction;  //TODO: спарсить ссылку

        //private int _factionID = -1;

        //public int FractionID
        //{
        //    get { return _factionID; }
        //    set
        //    {
        //        if (_factionID == -1) _factionID = value;
        //        else Debug.Log("Попытка повторной инициализации фракции. Фракции уже присвоен ID " + _factionID);
        //    }
        //}
        [SerializeField] private Transform _resourceListPivot;

        private Dictionary<GameEvents1.FactionResourcesChanged, Text> _resourceList = new Dictionary<GameEvents1.FactionResourcesChanged, Text>();
        private Dictionary<GameEvents1.FactionResourcesProfitChanged, Text> _resourceProfitList = new Dictionary<GameEvents1.FactionResourcesProfitChanged, Text>();

        string[] multiViewResourcesChangeEvents = Enum.GetNames(typeof(GameEvents1.FactionResourcesChanged));
        string[] multiViewResourcesProfitChangeEvents = Enum.GetNames(typeof(GameEvents1.FactionResourcesProfitChanged));


        /// <summary>
        ///Onvalidate, Awake, Start тут,
        ///OnDeastroy тоже
        /// </summary>
        #region Предустановки

        ///автопарсинг прямо из сцены, чтобы мозги не парить при дальнейшей разработке
        private void OnValidate()
        {
            if (_resourceListPivot == null) return;

            Transform tmp, tmp2; //tmp2 - нулевой GameObject с родителем tmp - должен содержать Event для прироста ресурса 
            GameEvents1.FactionResourcesChanged parsedEnumValue;
            GameEvents1.FactionResourcesProfitChanged parsedEnumValue2;
            for (int i = 0; i < _resourceListPivot.childCount; i++)
            {
                //т.к. иерархия имеет вид pivot / child(id)[картинка+наименование] / child(0)[значение ресурса]
                //добавляем в список ресурсов всех потомков потомков
                tmp = _resourceListPivot.GetChild(i);

                //пытаемся имя объекта преобразовать к перечислителю CityResources
                if (Enum.TryParse<GameEvents1.FactionResourcesChanged>(tmp.name, out parsedEnumValue)) //Аве 4+ .Net
                {
                    //Если получается, добавляем в справочник
                    _resourceList.Add(
                        parsedEnumValue,
                        tmp.GetChild(1).GetComponent<Text>());

                    tmp2 = tmp.GetChild(0);
                    if (Enum.TryParse<GameEvents1.FactionResourcesProfitChanged>(tmp2.name, out parsedEnumValue2))
                    {
                        _resourceProfitList.Add(
                            parsedEnumValue2,
                            tmp2.GetComponent<Text>());
                    }

                    //такие простые, но такие страшные и громоздкие операции. УУУУ
                }
            }
        }


        private void Awake()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < multiViewResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Trying to add Listener [" + cityResourcesChangeEvents[i] + "]");
                //handler = UpdateResource; //UpdateResource(int cityID, float value, string Event)
                Messenger<int, float>.AddListener(multiViewResourcesChangeEvents[i], UpdateResource);
            }
            for (int i = 0; i< multiViewResourcesProfitChangeEvents.Length; i++)
            {
                Messenger<int, float>.AddListener(multiViewResourcesProfitChangeEvents[i], UpdateProfit);
            }

            Messenger<bool>.AddListener(GameEvents.GUIGameEvent.BUTTON_STEP_SET_ACTIVE, UpdateAllResources);

            Messenger.AddListener(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT.ToString(), UpdateAllResources);
        }


        private void OnDestroy()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < multiViewResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Listener's number " + i);
                //handler = UpdateResource;
                Messenger<int, float>.RemoveListener(
                    multiViewResourcesChangeEvents[i], UpdateResource
                    ); //вызываем обобщенно, потом уже смотрим на строку ивента
            }
            for (int i = 0; i < multiViewResourcesProfitChangeEvents.Length; i++)
            {
                Messenger<int, float>.RemoveListener(multiViewResourcesProfitChangeEvents[i], UpdateProfit);
            }
            Messenger<bool>.RemoveListener(GameEvents.GUIGameEvent.BUTTON_STEP_SET_ACTIVE, UpdateAllResources);
            Messenger.RemoveListener(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT.ToString(), UpdateAllResources);
        }
        #endregion Предустановки

        /// </summary>
        /// <param name="cityResourceIndex"></param>
        /// <param name="cityID"></param>
        /// <param name="value"></param>
        private void UpdateResource(int factionID, float value, string MyEvent)
        {
            Debug.Log("factionID " + factionID + " value " + value + " MyEvent " + MyEvent);
            if  (_faction.ID != factionID) return;
            //если бот/другой игрок вызвал для своего другого города апдейт ресурсов, наш выбранный сейчас город это не колышет

            Text textGUI;
            GameEvents1.FactionResourcesChanged enumValue = (GameEvents1.FactionResourcesChanged)Enum.Parse(typeof(GameEvents1.FactionResourcesChanged), MyEvent);

            if (_resourceList.TryGetValue(enumValue, out textGUI))
            {
                float tmp = (int)(10f * value) / 10f;
                textGUI.text = tmp.ToString();
            }
            else Debug.Log("Для ресурса с ивентом " + MyEvent + " не существует визуального отображения на интерфейсе");
        }

        /// <summary>
        /// отображаем изменения профита
        /// </summary>
        /// <param name="factionID"></param>
        /// <param name="value"></param>
        /// <param name="MyEvent"></param>
        private void UpdateProfit(int factionID, float value, string MyEvent)
        {
            Debug.Log("factionID " + factionID + " value " + value + " MyEvent " + MyEvent);
            if (_faction.ID != factionID) return;
            //если бот/другой игрок вызвал для своего другого города апдейт ресурсов, наш выбранный сейчас город это не колышет

            Text textGUI;
            GameEvents1.FactionResourcesProfitChanged enumValue = (GameEvents1.FactionResourcesProfitChanged)Enum.Parse(typeof(GameEvents1.FactionResourcesProfitChanged), MyEvent);

            if (_resourceProfitList.TryGetValue(enumValue, out textGUI))
            {
                float tmp = (int)(10f * value) / 10f;

                //Debug.Log(textGUI.gameObject.name);
                if (value < 0f)
                { textGUI.text = tmp.ToString(); }
                else
                {
                    textGUI.text = "+" + tmp.ToString();
                }//ключевое отличие от UpdateResource
            }
            else Debug.Log("Для ресурса с ивентом " + MyEvent + " не существует визуального отображения на CityView");
        }

        private void UpdateAllResources ()
        { UpdateAllResources(true); }
        private void UpdateAllResources(bool isNewTurnStarted)
        {
            if (!isNewTurnStarted) return;

            if (_faction == null)  _faction = FactionsManager.GetUser();

            Debug.Log("Отслеживаемая в UI фракция:" +_faction.ID);
            Debug.Log("UpdateAllResources вызван");
            //Debug.Log("Food= " + city.CityResources.Food + ", FoodIncome=" + city.CityResources.FoodProfit);


            UpdateResource(_faction.ID, _faction.FactionResources.Money , GameEvents1.FactionResourcesChanged.MONEY_RESOURCE_CHANGED.ToString());


            UpdateProfit(_faction.ID, _faction.FactionResources.MoneyProfit , GameEvents1.FactionResourcesProfitChanged.FACTION_MONEY_PROFIT_RESOURCE_CHANGED.ToString());

        }

    }
}
