using UnityEngine;
using System.Collections;

/// <summary>
/// Сюда постепенно мигрируют все ивенты из GameEvenst
/// (вместо static const string теперь они имеют формат enum)
/// </summary>
public class GameEvents1
{

    public enum UIGameEvent
    {
        not_selected,
        //блок кнопок Units, Heroes и Cities
        ON_btnUnits,
        ON_btnHeroes,
        ON_btnCities,

        //блок кнопок отряда
        ON_btnAttack,
        ON_btnBuild,
        ON_btnDefffend,
        ON_btnRecrut,
        ON_btnMove,
        ON_btnMarch,
        ON_btnSkill1,
        ON_btnSkill2,
        ON_btnSkill3,

        //блок кнопок миникарты
        ON_btnGrid,
        ON_btnRes,
        ON_btnView,
        ON_btnCenter,
        ON_btnPlus,
        ON_btnMinus,

        //вызов конца хода
        ON_btnStep,

        //_________________
        //Игра отпраявляет сообщения-команды, GUI реагирует
        //можно было на изменение написать, но я решил, так будет "отказоустойчивее"
        BUTTON_STEP_SET_ACTIVE,
        SET_AVATAR,
        UPDATE_RESOURCES,
        LOAD_STORY
    }

    public enum UICityEvent
    {
        not_selected,

        //HUD обзора города
        //GUI-кнопки или хоткеи отправляют сообщения-команды, игра реагирует на них
        ON_brtClose,

        //блок кнопок Creation
        SELECT_BUILDING_PANEL,
        SELECT_UNIT_PANEL,
        SELECT_CARAVAN_PANEL,

        //кнопки на панели создания
        ON_btnBuilding,
        ON_btnUnit,
        ON_btnCaravan,

        //кнопка "Подробное инфо прибыли города"
        ON_Btn_ShowIncomeInfo,
        //____________
        // на вход интерфейсу города подаются
        // lblCity - имя города

        //кнопки паузы/снятия паузы и отмены строительства
        ON_btnBuildingPause,
        ON_btnBuildingUnpause,
        ON_btnBuildingCancel,
        ON_btnBuildingBuyout,

        BUILDING_PLACE_SELECTED,

        UPDATE_BUIDING_LIST,
        UPDATE_UNIT_LIST
        // tabBuilding - список "ключей" от доступных построек. По ключам GUI-менеджер будет подгружать картинку+текст+стоимость и помещать полученный объект в скроллер
        // tabUnit - список "ключей" от доступных для найма юнитов. Аналогично tabBuilding
        // tabCaravan - список "ключей" от доступных для найма караванов. Аналогично tabBuilding
    }



    //городские ресурсы
    public enum CityResourcesChanged
    {
        FOOD_RESOURCE_CHANGED,
        MATERIALS_RESOURCE_CHANGED,
        POPULATION_RESOURCE_CHANGED
    }
    public enum CityResourcesProfitChanged
    {
        FOOD_PROFIT_CHANGED,
        CITY_MONEY_PROFIT_CHANGED
    }

    //фракционные ресурсы
    public enum FactionResourcesChanged
    {
        MONEY_RESOURCE_CHANGED,
        WOOD_RESOURCE_CHANGED,
        STONE_RESOURCE_CHANGED,
        IRON_RESOURCE_CHANGED,
        HIDE_RESOURCE_CHANGED,
        CATTLES_RESOURCE_CHANGED,
        POPULATION_SUMMARY_RESOURCE_CHANGED,
        HORSE_RESOURCE_CHANGED,
        MANASTONE_RESOURCE_CHANGED,
        MITHRIL_RESOURCE_CHANGED,
        CHARCOAL_RESOURCE_CHANGED,
        FACTION_RESOURCE_CHANGED
    }
    public enum FactionResourcesProfitChanged
    {
        FACTION_MONEY_PROFIT_RESOURCE_CHANGED
    }
}