using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using WAR.GameEngine;
using UnityEngine.SceneManagement;

public class CityView : MonoBehaviour
{
    public GameObject GameView;

    public GameObject tabDistrict;
    public GameObject tabBuilding;
    public GameObject tabHornizon;

    public GameObject tabCreateBuilding;
    public GameObject tabCreateUnit;
    public GameObject tabCreateCaravan;

    public GameObject Managment;

    public Text cityName;

    public Transform _content;
    public City city;
    public GameObject unitButton;

    private List<GameObject> _DisplayedGameObjectList;

    public Button unitConstructorButton;

    public void Close()
    {
        Messenger.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE);
        //this.gameObject.SetActive(false);
        //GameView.SetActive(true);
    }

    public void OpenTabDistrict()
    {
        tabDistrict.SetActive(true);
        tabBuilding.SetActive(false);
        tabHornizon.SetActive(false);
    }

    public void OpenTabBuilding()
    {
        tabDistrict.SetActive(false);
        tabBuilding.SetActive(true);
        tabHornizon.SetActive(false);
    }

    public void OpenTabHornizon()
    {
        tabDistrict.SetActive(false);
        tabBuilding.SetActive(false);
        tabHornizon.SetActive(true);
    }


    public void OpenTabCreateBuilding()
    {
        tabCreateBuilding.SetActive(true);
        tabCreateUnit.SetActive(false);
        tabCreateCaravan.SetActive(false);
    }

    public void OpenTabCreateUnit()
    {
        tabCreateBuilding.SetActive(false);
        tabCreateUnit.SetActive(true);
        tabCreateCaravan.SetActive(false);
    }

    public void OpenTabCreateCaravan()
    {
        tabCreateBuilding.SetActive(false);
        tabCreateUnit.SetActive(false);
        tabCreateCaravan.SetActive(true);
    }

    public void CloseManagmentPanel()
    {
        Managment.GetComponent<Animator>().SetInteger("Close", 1);
    }

    public void OpenManagmentPanel()
    {
        Managment.GetComponent<Animator>().SetInteger("Close", 0);
    }

    public void OpenUnitConstructorScene()
    {
        GameObject.FindObjectOfType<GodBehaviour>().inerface.gameObject.SetActive(false);
        
        SceneManager.LoadScene("UnitConstructor", LoadSceneMode.Additive);

        //GameObject.FindObjectOfType<ArmoryContentMaanger2>().RecreateUnit(this.city.gameObject.GetComponent<CityBeahaviour>().unitsAvailable[0]);
    }

    public void ShowWindow(City city)
    {
        this.city = city;
        cityName.text = city.name;
        this.gameObject.SetActive(true);

        UpdateList(city.cityBeh.Unit);//this.city.cityBeh.unitsAvailable);
    }

    public void HideWindow()
    {
        this.gameObject.SetActive(false);
    }


    /// <summary>
    /// Скрипт удалит старый список отображаемых построек и построит новый на основе входных данных
    /// </summary>
    /// <param name="AvaiableBuildings">Список доступных построек</param>
    /// <param name="NotAvaiableBuildings">Список недоступных построек, которые нужно отобразить</param>
    /// <returns></returns>
    void UpdateList(UnitInSquadController currentUnit)//List<UnitInSquadController> AvaiableUnits)
    {
        //метод должен сработать только для активного окна (только для нужного города)
        //если gameObject текущего списка не активен, ничего не делать
        //if (!this.gameObject.activeInHierarchy)
        //        return;

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

        if (currentUnit == null)//AvaiableUnits == null)
        {
            Debug.Log("На вход UpdateList подан пустой список");
            return;
        }//


         //   var go = Instantiate(unitButton, _content);
        //go.GetComponent<Image>().sprite = currentUnit.unitIcon;//AvaiableUnits[i].texture;

        //    var buttonScript = go.GetComponent<UnitCreateOnBuildPanel>();

        //    buttonScript.unitPrefab = currentUnit;//AvaiableUnits[i];
        //    buttonScript.city = this.city;

       //     _DisplayedGameObjectList.Add(go);

            //_DisplayedGameObjectList.Add(Instantiate(_testRing.Current, _content)); 
        //}
    }

}
