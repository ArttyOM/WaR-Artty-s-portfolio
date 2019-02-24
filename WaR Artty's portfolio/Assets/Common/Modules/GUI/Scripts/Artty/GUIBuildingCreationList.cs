using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WAR;
using WAR.GameEngine;
using DBTypes;


public class GUIBuildingCreationList : MonoBehaviour {

    [SerializeField] protected Transform _content;
    [SerializeField] protected Transform _unitContent;

    [SerializeField] protected GameObject defaultButton;
    protected List<GameObject> _DisplayedGameObjectList;
    private List<GameObject> _displayedQueueElements;

    public static IEnumerable<SBuilding> buildings;
    public static IEnumerable<DBTypes.Unit> units;



    public static City lastCity;
    void Awake()
    {
        Messenger<City>.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, UpdateList);
        Messenger.AddListener(GameEvents.CityGUIGameEvent.BUILDING_PLACE_SELECTED, InstantiateSingleBuildingInProgress);

    }
    void OnDestroy()
    {
        Messenger<City>.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, UpdateList);
        Messenger.RemoveListener(GameEvents.CityGUIGameEvent.BUILDING_PLACE_SELECTED, InstantiateSingleBuildingInProgress);
    }


    /// <summary>
    /// Скрипт удалит старый список отображаемых построек и построит новый на основе входных данных
    /// </summary>
    /// <param name="AvaiableBuildings">Список доступных построек</param>
    /// <param name="NotAvaiableBuildings">Список недоступных построек, которые нужно отобразить</param>
    /// <returns></returns>
    void UpdateList(City city)
    {
        //Debug.Log("Вызван UpdateList(city), " + city.ToString());
        lastCity = city;
        InstantiateBuildingsInProgress(city);
        //метод должен сработать только для активного окна (только для нужного города)
        //если gameObject текущего списка не активен, ничего не делать
        //if (!this.gameObject.activeInHierarchy)
        //        return;

        //_______________ лезем в БД за списком
        DataService ds = DataService.WARSQL;
        buildings = ds.GetBuildingsWithImage();
        units = ds.GetUnits();
        //________________

        //удаляем все, что отображалось до этого
        if (_DisplayedGameObjectList != null)
        {
            foreach (GameObject gObj in _DisplayedGameObjectList)
            {
                Destroy(gObj);
            }
            _DisplayedGameObjectList.Clear();
        }   //зачистим список
        else _DisplayedGameObjectList = new List<GameObject>();


        //if (AvaiableBuildings == null)
        //    {
        //        Debug.Log("На вход UpdateList подан пустой список");
        //        return;
        //    }//

        GameObject tmpGObj;//= new GameObject();

        if (buildings != null)
        {
            foreach (SBuilding building in buildings)
            {
                //Debug.Log(building.ToString());
                //TODO вынести в фабричный метод
                building.SetStatus(Status.New);
                building.SetOwner(city);

                tmpGObj = Instantiate(defaultButton, _content);
                tmpGObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(building.pathToImage);
                tmpGObj.AddComponent<Creation_BtnBuilding>().building = building;

                _DisplayedGameObjectList.Add(tmpGObj);
                //ToConsole(person.ToString());
            }
        }

        if (units != null)
        {
            foreach (DBTypes.Unit unit in units)
            {
                tmpGObj = Instantiate(defaultButton, _unitContent);
                tmpGObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(unit.PathToImage);
                //tmpGObj.AddComponent<Creation_BtnUnit>().unit = unit;
                UnitCreateOnBuildPanel btnScript = UnitCreateOnBuildPanel.InstantiateUnitBtn(tmpGObj, city.cityBeh.Unit, city, unit);

                Debug.Log(unit.MoneyUpkeep);

                _DisplayedGameObjectList.Add(tmpGObj);
            }
        }

    }

    //private void Update()
    //{
    //    if ((Input.GetKey(KeyCode.B)) && (lastCity!=null))
    //    {
    //        InstantiateBuildingsInProgress(lastCity);
    //    }
    //}

    private void InstantiateSingleBuildingInProgress()
    {
        if (lastCity == null)
        {
            Debug.Log("Последний выбранный город отсутствует");
            return;
        }

        //приходится полностью обновлять очередь, т.к. BuildingSystem.GetBuildingOrderForCity() изменился
        //TODO: переписать обновление очереди без уничтожения объектов!
        InstantiateBuildingsInProgress(lastCity);
    }

    void InstantiateBuildingsInProgress(City city)
    {
        //Debug.Log(city.ToString());
        if (_displayedQueueElements != null)
        {
            foreach (GameObject gObj in _displayedQueueElements)
            {
                Destroy(gObj);
            }
            _displayedQueueElements.Clear();
        }   //зачистим список
        else _displayedQueueElements = new List<GameObject>();

        List<BuildingInProgressInfo> buildingInfoList =  city.BuildingSystem.GetBuildingOrderForCity();
        foreach (BuildingInProgressInfo info in buildingInfoList)
        {
            _displayedQueueElements.Add(CreationQueueElement.InstantiateQueueElement(info));
        }
    }

    //void OnBtnBuilding(int myBuilding)
    //{

    //    Debug.Log(gameObject.name);
    //    Messenger<int>.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING, myBuilding); //(int)Building.Buildings.TownHall_lvl0);

    //}

    //IEnumerator AutoAddBuildingToScroller()
    //{

    //    //foreach (GameObject gObj in _actualList)
    //    //{
    //    //    Instantiate(gObj, _content);
    //    //    yield return new WaitForSeconds(2f);
    //    //}
    //   // Debug.Log(_testRing.index);
    //    //foreach (GameObject gObj in _testRing)
    //    for (;;)
    //    {
    //        //Debug.Log("debug1 "+_testRing.index);

    //        Instantiate(_testRing.Current, _content);

    //        _testRing.MoveNext();
    //        //Instantiate(_actualList);
    //        yield return new WaitForSeconds(2f);
    //    }
    //}
}

