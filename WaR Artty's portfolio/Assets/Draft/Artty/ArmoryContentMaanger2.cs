using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Data;
using WAR;
using WAR.GameEngine;



public class ArmoryContentMaanger2 : MonoBehaviour
{
    [Serializable]
    public struct kostyl
    {
         public HandEquip item;
         public EquipmentType slot;
    }

    /// <summary>
    /// Трансформ родительского объекта, ниже по иерархии которого находятся все объекты экипировки
    /// </summary>
    [SerializeField] private Transform _content;

    /// <summary>
    /// на текущий момент не востребован, в будущем - один из элементов фильтра экипировки по типам
    /// </summary>
    [SerializeField] private Dropdown _dropdown;

    /// <summary>
    /// префаб пустышка, задает "эталон" размера иконки на канвасе
    /// </summary>
    [SerializeField] private GameObject _pref;

    /// <summary>
    /// на текущий момент не востребован
    /// </summary>
    [SerializeField] private Sprite sprite;

    public CityBeahaviour cityBeh;

    //public List<HandEquip> items;

    public List<kostyl> kitems;

    // базовый юнит, с которого берем расположение и врщение
    public UnitInSquadController unit;

    public void CloseScene()
    {
        var god = GameObject.FindObjectOfType<GodBehaviour>();
        god.inerface.cityView.ShowWindow(god.inerface.cityView.city);
        god.inerface.gameObject.SetActive(true);
        unit.transform.position = Vector3.zero;
        unit.gameObject.SetActive(false);



        SceneManager.UnloadSceneAsync("UnitConstructor");
    }

    private void Awake()
    {
        for (int i = 0; i < kitems.Count; i++)
        {
            GameObject tmp = Instantiate(_pref, _content);
            Equipment.CreateComponent(tmp, kitems[i].item.name, kitems[i].slot, kitems[i].item.sprite, kitems[i].item);
        }

        var god = GameObject.FindObjectOfType<GodBehaviour>();
        if (god == null) return;

        this.cityBeh = god.inerface.cityView.city.gameObject.GetComponent<CityBeahaviour>();

        this.RecreateUnit(cityBeh.Unit);

        ///Artty - я хз что делает этот код. Но если корректно не работает определение позиции оружия в WRWorld - по-любому косяк тут: WAR.EquipmentType.weapon 
        ///(весь эквип, в т.ч щиты и шлема, считаются оружием в основной руке)
        for (int i = 0; i < cityBeh.unitsAvailable.Count; i++)
        {
            GameObject tmp = Instantiate(_pref, _content);
            var equip = Equipment.CreateComponent(tmp, cityBeh.unitsAvailable[i].name, WAR.EquipmentType.weapon, cityBeh.unitsAvailable[i].armorIcon, null, cityBeh.unitsAvailable[i]);
        }

        //var cityVew = GameObject.FindObjectOfType<CityView>();
        //var cityBeh = cityVew.city.gameObject.GetComponent<CityBeahaviour>();

        //this.RecreateUnit(cityBeh.unitsAvailable[0]);
    }

    public void RecreateUnit(UnitInSquadController unitprest)
    {
        // если уже был создан пресет, то оружие надо брать с него
        if (cityBeh.unit != null)
            unit = cityBeh.Unit;

        // взяли инфу с базового юнита для этой сцены
        this.unit.transform.position = new Vector3(-0.2f, 1041, -109);

        var scaleFactor = this.unit.transform.localScale;

        var position = this.unit.transform.position;
        var rotation = this.unit.transform.rotation;
        Destroy(this.unit.gameObject);

        // клонируем оружие с префаба юнита для спауна
        var oldWeaponfromLH = GameObject.Instantiate(unit.leftHand, unit.leftHand.transform.position, unit.leftHand.transform.rotation, unit.leftHand.transform);
        var oldWeaponfromRH = GameObject.Instantiate(unit.RightHand, unit.RightHand.transform.position, unit.RightHand.transform.rotation, unit.RightHand.transform);

        oldWeaponfromLH.transform.SetParent(null);
        oldWeaponfromRH.transform.SetParent(null);

        oldWeaponfromLH.transform.localScale = new Vector3(oldWeaponfromLH.transform.localScale.x * scaleFactor.x,
                                                           oldWeaponfromLH.transform.localScale.y * scaleFactor.y,
                                                           oldWeaponfromLH.transform.localScale.z * scaleFactor.z);

        oldWeaponfromRH.transform.localScale = new Vector3(oldWeaponfromRH.transform.localScale.x * scaleFactor.x,
                                                   oldWeaponfromRH.transform.localScale.y * scaleFactor.y,
                                                   oldWeaponfromRH.transform.localScale.z * scaleFactor.z);

        unit = Instantiate<UnitInSquadController>(unitprest);
        unit.gameObject.SetActive(true);
        unit.transform.position = position;
        unit.transform.rotation = rotation;

        if (oldWeaponfromRH != null)
        {
            oldWeaponfromRH.transform.SetParent(unit.RightHand.transform.parent);
            oldWeaponfromRH.transform.position = unit.RightHand.transform.position;
            oldWeaponfromRH.transform.rotation = unit.RightHand.transform.rotation;
        }
        if (oldWeaponfromLH != null && unit.leftHand != null)
        {
            oldWeaponfromLH.transform.SetParent(unit.leftHand.transform.parent);
            oldWeaponfromLH.transform.position = unit.leftHand.transform.position;
            oldWeaponfromLH.transform.rotation = unit.leftHand.transform.rotation;
        }

        if (unit.leftHand != null)
            DestroyImmediate(unit.leftHand.gameObject);

        if (unit.RightHand != null)
            DestroyImmediate(unit.RightHand.gameObject);

        unit.leftHand = oldWeaponfromLH;
        unit.RightHand = oldWeaponfromRH;

        // изменили префаб юнита для спауна
        cityBeh.Unit = unit;
    }

    public void ChangeAmunition(HandEquip handequip)
    {
        var unitInSquad = unit.GetComponent<UnitInSquadController>();

        unit.GetComponent<Animator>().applyRootMotion = false;

        if (handequip.equipType != HandEquipType.shield)
        {
            if (unit.RightHand != null)
            {
                var weaponGO = GameObject.Instantiate(handequip);

                weaponGO.transform.SetParent(unit.RightHand.transform.parent.transform);

                weaponGO.transform.localPosition = unit.RightHand.transform.localPosition;
                weaponGO.transform.localRotation = unit.RightHand.transform.localRotation;
                weaponGO.transform.localScale = unit.RightHand.transform.localScale;

                DestroyImmediate(unit.RightHand);
                unit.RightHand = weaponGO.gameObject;
            }
        }
        else
        {
            if (unit.leftHand != null)
            {
                var armorGO = GameObject.Instantiate(handequip);
                armorGO.transform.SetParent(unit.leftHand.transform.parent.transform);

                armorGO.transform.localPosition = unit.leftHand.transform.localPosition;
                armorGO.transform.localRotation = unit.leftHand.transform.localRotation;
                armorGO.transform.localScale = unit.leftHand.transform.localScale;

                DestroyImmediate(unit.leftHand);
                unit.leftHand = armorGO.gameObject;
            }
        }

        if (handequip.equipType == HandEquipType.bow)
        {
            var armorGO = new GameObject("empty");
            armorGO.transform.SetParent(unit.leftHand.transform.parent.transform);

            armorGO.transform.localPosition = unit.leftHand.transform.localPosition;
            armorGO.transform.localRotation = unit.leftHand.transform.localRotation;
            armorGO.transform.localScale = unit.leftHand.transform.localScale;

            DestroyImmediate(unit.leftHand);
            unit.leftHand = armorGO.gameObject;
        }

        // изменили префаб юнита для спауна
        cityBeh.Unit = unit;
    }

    public void ChangeArmor(UnitInSquadController unit)
    {

    }

}