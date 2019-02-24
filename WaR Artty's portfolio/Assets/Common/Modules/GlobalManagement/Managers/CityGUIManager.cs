using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WAR.GameEngine;

public class CityGUIManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    [SerializeField] private Image _tabBuilding;
    [SerializeField] private Image _tabUnit;
    [SerializeField] private Image _tabCaravan;

    [SerializeField] private GameObject _creationQueue;

    public static GameObject creationQueue { get; private set; }
    //BuildingsDictionary buildDictuionary = BuildingsDictionary.GetInstance();
    //private List<string> _inputList;
    public void Startup()
    {
        Debug.Log("GUI manager started...");

        if (creationQueue == null)
            creationQueue = _creationQueue;

        status = ManagerStatus.Started;
    }
    
    //когда игрок открывает панель города
    public void OnCityViewOpen()
    {
        OpenBuildingPanel(); //по умолчанию открыта вкладка построек
    }


    
    //блок кнопок Creation
    public void OpenBuildingPanel()
    {
        _tabBuilding.gameObject.SetActive(true);
        _tabUnit.gameObject.SetActive(false);
        _tabCaravan.gameObject.SetActive(false);
    }
    public void OpenUnitPanel()
    {
        _tabBuilding.gameObject.SetActive(false);
        _tabUnit.gameObject.SetActive(true);
        _tabCaravan.gameObject.SetActive(false);
    }
    public void OpenCaravanPanel()
    {
        _tabBuilding.gameObject.SetActive(false);
        _tabUnit.gameObject.SetActive(false);
        _tabCaravan.gameObject.SetActive(true);
    }



}
