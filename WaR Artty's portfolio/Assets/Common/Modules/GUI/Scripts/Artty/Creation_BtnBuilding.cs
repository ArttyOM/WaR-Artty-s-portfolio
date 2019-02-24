using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WAR;
using DBTypes;
public class Creation_BtnBuilding : MonoBehaviour {
    //public Building.Buildings key;
    public SBuilding building;
    


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { OnBtnBuilding(building); });
    }

    //private void OnValidate()
    //{
    //    //_queuePrefab = Resources.Load<GameObject>("_default_queueLine");
    //    //_content = GameObject.Find("QueueContent");
    //}

    public void OnBtnBuilding(SBuilding myBuilding)
    {
        //Debug.Log("Вызван OnBtnBuildong, " + myBuilding.ToString());
        //Messenger<int>.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING, myBuilding); //(int)Building.Buildings.TownHall_lvl0);
        Messenger<SBuilding>.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING, myBuilding);
        //Instantiate(_queuePrefab, _content.transform);
        //CreationQueueElement.InstantiateQueueElement(building);
    }
}
