using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WAR;
using DBTypes;
//Скрипт для кнопки "создать Юнита"
public class Creation_BtnUnit : MonoBehaviour
{

    public DBTypes.Unit unit;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { OnBtnUnit(unit); });
    }

    public void OnBtnUnit(DBTypes.Unit myUnit)
    {
        Messenger<DBTypes.Unit>.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_UNIT, myUnit);
    }
}
