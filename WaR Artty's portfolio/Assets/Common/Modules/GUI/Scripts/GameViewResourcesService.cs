using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using WAR.GameEngine;

namespace WAR
{
    /// <summary>
    /// Обновляет фракционные ресурсы на соответствующей плашке в GameView
    /// 
    /// </summary>
    public class GameViewResourcesService : MonoBehaviour
    {
        private int _fractionID = -1;
        public int FractionID
        {
            get {return _fractionID;}
            set {
                if (_fractionID == -1) _fractionID = value;
                else Debug.Log("Попытка повторной инициализации фракции. Фракции уже присвоен ID " + _fractionID);
            }
        }
        [SerializeField] private Transform _resourceListPivot;

        //TODO: сейчас в сцене стоит только один корректный ресурс (с ивентом MONEY_RESOURCE_CHANGED) -> в словарь попадет только он. 
        //От ГД нужна инфа, какие ресурсы отображать на GUI, от художников спрайты для них
        private Dictionary<GameEvents1.FactionResourcesChanged, Text> _resourceList = new Dictionary<GameEvents1.FactionResourcesChanged, Text>();

        string[] gameViewResourcesChangeEvents = Enum.GetNames(typeof(GameEvents1.FactionResourcesChanged));


        /// <summary>
        ///Onvalidate, Awake, Start тут,
        ///OnDeastroy тоже
        /// </summary>
        #region Предустановки

        ///автопарсинг прямо из сцены, чтобы мозги не парить при дальнейшей разработке
        private void OnValidate()
        {
            if (_resourceListPivot == null) return;
            Transform tmp;
            GameEvents1.FactionResourcesChanged parsedEnumValue;
            for (int i = 0; i < _resourceListPivot.childCount; i++)
            {
                //т.к. иерархия имеет вид pivot / child(id)[наименование+картинка] / child(0)[значение ресурса]
                //добавляем в список ресурсов всех потомков потомков
                tmp = _resourceListPivot.GetChild(i);

                //пытаемся имя объекта преобразовать к перечислителю CityResources
                if (Enum.TryParse<GameEvents1.FactionResourcesChanged>(tmp.name, out parsedEnumValue)) //Аве 4+ .Net
                {
                    //Если получается, добавляем в справочник
                    _resourceList.Add(
                        parsedEnumValue,
                        tmp.GetChild(0).GetComponent<Text>());
                    //такие простые, но такие страшные и громоздкие операции. УУУУ
                }
            }
        }


        private void Awake()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < gameViewResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Trying to add Listener [" + cityResourcesChangeEvents[i] + "]");
                //handler = UpdateResource; //UpdateResource(int cityID, float value, string Event)
                Messenger<int, float>.AddListener(gameViewResourcesChangeEvents[i], UpdateResource);
            }
        }

        private void OnDestroy()
        {
            //Action<int, float, string> handler;
            for (int i = 0; i < gameViewResourcesChangeEvents.Length; i++)
            {
                //Debug.Log("Listener's number " + i);
                //handler = UpdateResource;
                Messenger<int, float>.RemoveListener(
                    gameViewResourcesChangeEvents[i], UpdateResource
                    ); //вызываем обобщенно, потом уже смотрим на строку ивента
            }
        }
        #endregion Предустановки


        private void UpdateResource(int fractionID, float value, string MyEvent)
        {
            //Debug.Log("cityID " + cityID + " value " + value + " MyEvent " + MyEvent);
            if ((_fractionID == -1) || (_fractionID != fractionID)) return;
            //если бот/другой игрой вызвал для своей фракции апдейт ресурсов, нас это не колышет

            Text textGUI;
            GameEvents1.FactionResourcesChanged enumValue = (GameEvents1.FactionResourcesChanged)Enum.Parse(typeof(GameEvents1.FactionResourcesChanged), MyEvent);

            if (_resourceList.TryGetValue(enumValue, out textGUI))
            {
                textGUI.text = value.ToString();
            }
            else Debug.Log("Для ресурса с ивентом "+ MyEvent+ " не существует визуального отображения на GameView");
        }
    }
}