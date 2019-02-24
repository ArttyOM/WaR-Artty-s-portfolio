using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WAR.GameEngine;

public class UnitCreateOnBuildPanel : MonoBehaviour
{

    public static UnitCreateOnBuildPanel InstantiateUnitBtn(GameObject gObj,  UnitInSquadController unitPrefab, City city, DBTypes.Unit unit)
    {
        UnitCreateOnBuildPanel result = gObj.AddComponent<UnitCreateOnBuildPanel>();
        result.UPrefab = unitPrefab;
        result.UCity = city;
        result.UnitParams = unit;

        return result;
    }

    public UnitInSquadController UPrefab { get; private set; }
    public City UCity { get; private set; }

    public DBTypes.Unit UnitParams { get; private set; }

    private Button _button;

    

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (!_button) _button = gameObject.AddComponent<Button>();

        _button.onClick.AddListener(delegate { CreateUnit(); });

        BlockIfNotEnoughMoney();

        Messenger.AddListener(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT, BlockIfNotEnoughMoney);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT, BlockIfNotEnoughMoney); 
    }

    public void BlockIfNotEnoughMoney()
    {
          if (UCity.faction.FactionResources.Money < UnitParams.BuybackCost)
            _button.interactable = false;
    }

    public void CreateUnit()
    {
        if (UCity.center.Squad == null)
            UCity.faction.currentActor.objManager.CreateSquad(UCity, UCity.center, SquadFormationName.normal9, false, UCity.cityBeh.Unit, UnitParams);

        UCity.faction.FactionResources.Money -= UnitParams.BuybackCost;
        //Отправляем на GUI команду отобразить новые деньги и себе же - если денег не хватает, нужно скрыть кнопку, а раз у нас уже есть листенер - им и воспльзуемся
        Messenger.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT.ToString());
        Debug.Log(UCity.faction.FactionResources.Money);
    }
}
