using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR;

/// <summary>
/// скрипт дает команду вывести на экран все постройки из БД
/// </summary>
public class TestMessenger2 : MonoBehaviour {
    // Vit: я сломаль =( Энума больше нет
    /*
    List<Building.Buildings> testList = new List<Building.Buildings>();
    public Building_GUICreationPanelDB BuildingDatabase;
	// Use this for initialization
	void Start () {

        if (BuildingDatabase == null) BuildingDatabase = Resources.Load("Buildings_GUICreationPanelDB") as Building_GUICreationPanelDB;

        for (int i = 0; i< BuildingDatabase.buildings.Count; i++)
        {
            testList.Add(BuildingDatabase.buildings[i].key);
        }
        if (testList == null) return;

        //Debug.Log(testList[0]);
        //Debug.Log(testList[1]);
        Messenger<List<Building.Buildings>>.Broadcast(GameEvents.CityGUIGameEvent.UPDATE_BUIDING_LIST, testList);
	}
	*/
	// Update is called once per fram



}
