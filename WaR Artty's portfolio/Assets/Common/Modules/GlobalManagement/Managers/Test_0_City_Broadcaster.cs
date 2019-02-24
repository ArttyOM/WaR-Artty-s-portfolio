using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR;
using System;
using System.Linq;
/// <summary>
/// тестовый скрипт, можно безболезненно вырубать
/// устанавливает Food = 2 и "Science = 4.33f
/// </summary>
public class Test_0_City_Broadcaster : MonoBehaviour {

 

    Dictionary<GameEvents1.CityResourcesChanged, float> resourceDict = new Dictionary<GameEvents1.CityResourcesChanged, float>();


    void Start()
    {
        resourceDict.Add(GameEvents1.CityResourcesChanged.POPULATION_RESOURCE_CHANGED, 2f);
       // resourceDict.Add(GameEvents1.CityResourcesChanged.MONEY_INCOME_RESOURCE_CHANGED, -4.33f);
        resourceDict.Add(GameEvents1.CityResourcesChanged.MATERIALS_RESOURCE_CHANGED, 1f);

        //Debug.Log("Записи в справочнике");
        //foreach (KeyValuePair<string, float> resource in resourceDict)
        //{
        //    Debug.Log(resource.Key + " " + resource.Value);
        //}
        //Debug.Log("отправлено сообщение");

        //Messenger<Dictionary<CityResources, float>>.Broadcast(GameEvents.UICityEvent.UPDATE_CITY_RESOURCES.ToString(), resourceDict);

        GameEvents1.CityResourcesChanged tmpKey = GameEvents1.CityResourcesChanged.FOOD_RESOURCE_CHANGED;
        float tmpValue = -900f;

        foreach (KeyValuePair<GameEvents1.CityResourcesChanged, float> resource in resourceDict)
        {
            tmpKey = resource.Key;
            tmpValue = resource.Value;
            //Debug.Log(resource.Key + " " + resource.Value);
            //Debug.Log(resource.Key.ToString());

            Messenger<int, float>.Broadcast(resource.Key.ToString(), 0/*Для GUI пока что не важен ID города*/, resource.Value); 
        }

        //Messenger<int, float>.Broadcast(tmpKey.ToString(), 0/*Для GUI пока что не важен ID города*/, tmpValue);





    }
}
