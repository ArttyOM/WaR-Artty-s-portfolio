using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text.RegularExpressions;
using System.Linq;

using WAR;
//using WAR.LandEngine;


[CreateAssetMenu(fileName = "Buildings_GUICreationPanelDB", menuName = "GUICreationPanelDB/Building")]
[System.Serializable]
public class Building_GUICreationPanelDB : ScriptableObject
{
    [SerializeField]
    public List<BuildingDBElement> buildings;


}

//вынести в отдельный скрипт
[Serializable]
public struct BuildingDBElement
{
    //public WAR.Building.Buildings key;
    public GameObject buttonPrefab;
    public ScriptableObject buildingSO;
}

[CreateAssetMenu(fileName = "Units_GUICreationPanelDB", menuName = "GUICreationPanelDB/Units")]
[System.Serializable]
public class Units_GUICreationPanelDB : ScriptableObject
{
    [SerializeField]
    public List<UnitsDBElement> units;
}

//вынести в отдельный скрипт
[Serializable]
public struct UnitsDBElement
{
    public UnitTypes id_type;
    public UnitLands id_land;
    public GameObject buttonPrefab;
}

//from Artty to Maewyn - перенеси плз enum в свои скрипты
//у нас enum-ы building вынесены, юнитов тоже надо вынести
[Serializable]
public enum UnitTypes
{
    //poor,
    leather_lvl0,
    hauberg_lvl1

}
public enum UnitLands
{
    sand,
    snow,
    sod
}



