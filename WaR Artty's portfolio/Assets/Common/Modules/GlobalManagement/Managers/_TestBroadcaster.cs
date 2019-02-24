using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// тестовый скрипт, можно безболезненно вырубать
/// устанавливает Food = 2 и "Science = 4.33f
/// </summary>
public class _TestBroadcaster : MonoBehaviour {

    Dictionary<string, float> resourceDict = new Dictionary<string, float>();

    List<string> buildingKeyList = new List<string>();


    void Start()
    {
        resourceDict.Add("Food", 2f);
        resourceDict.Add("Science", 4.33f);
        resourceDict.Add("flugegeheimen", 1f);

        //Debug.Log("Записи в справочнике");
        //foreach (KeyValuePair<string, float> resource in resourceDict)
        //{
        //    Debug.Log(resource.Key + " " + resource.Value);
        //}
        BuildingsDictionary buildDictionary = BuildingsDictionary.GetInstance();
        //foreach(Building building in buildDictionary.dict)
        //{
        //    buildingKeyList.Add(building.key);
        //}


        //Debug.Log("отправлено сообщение");
        Messenger<Dictionary<string, float>>.Broadcast(GameEvents.GUIGameEvent.UPDATE_RESOURCES, resourceDict);

        //Отправляем список ключей доступных построек
        //Messenger<List<string>>.Broadcast(GameEvents.CityGUIGameEvent.UPDATE_BUIDING_LIST, buildingKeyList);
       

    }
}
