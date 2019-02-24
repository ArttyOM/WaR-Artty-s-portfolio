using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents  {
    /// <summary>
    /// Справочник событий для GUI-менеджера (окно города)
    /// Artty: можно выпиливать
    /// </summary>
    public class CityGUIGameEvent
    {

        //HUD обзора города
        //GUI-кнопки или хоткеи отправляют сообщения-команды, игра реагирует на них
        public const string ON_BUTTON_CLOSE = "ON_brtClose";

        //блок кнопок Creation
        public const string SELECT_BUILDING_PANEL = "SELECT_BUILDING_PANEL";
        public const string SELECT_UNIT_PANEL = "SELECT_UNIT_PANEL";
        public const string SELECT_CARAVAN_PANEL = "SELECT_CARAVAN_PANEL";

        //кнопки на панели создания
        public const string ON_BUTTON_BUILDING = "ON_btnBuilding";
        public const string ON_BUTTON_UNIT = "ON_btnUnit";
        public const string ON_BUTTON_CARAVAN = "ON_btnCaravan";
        //____________
        // на вход интерфейсу города подаются
        // lblCity - имя города

        //кнопки паузы/снятия паузы и отмены строительства
        public const string ON_BUTTON_BUILDING_PAUSE = "ON_btnBuildingPause";
        public const string ON_BUTTON_BUILDING_UNPAUSE = "ON_btnBuildingUnpause";
        public const string ON_BUTTON_BUILDING_CANCEL = "ON_btnBuildingCancel";
        public const string ON_BUTTON_BUILDING_BUYOUT = "ON_btnBuildingBuyout";

        public const string BUILDING_PLACE_SELECTED = "BUILDING_PLACE_SELECTED";

        public const string UPDATE_BUIDING_LIST = "UPDATE_BUIDING_LIST";
        public const string UPDATE_UNIT_LIST = "UPDATE_UNIT_LIST";
        // tabBuilding - список "ключей" от доступных построек. По ключам GUI-менеджер будет подгружать картинку+текст+стоимость и помещать полученный объект в скроллер
        // tabUnit - список "ключей" от доступных для найма юнитов. Аналогично tabBuilding
        // tabCaravan - список "ключей" от доступных для найма караванов. Аналогично tabBuilding
    }

    /// <summary>
    /// Справочник событий для GUI-менеджера (основное игровое окно)
    /// 
    /// Artty: можно выпиливать
    /// </summary>
    public class GUIGameEvent
    {
        //Основной HUD
        //GUI-кнопки или хоткеи отправляют сообщения-команды, игра реагирует на них

        //блок кнопок Units, Heroes и Cities
        public const string ON_BUTTON_UNITS = "ON_btnUnits";
        public const string ON_BUTTON_HEROES = "ON_btnHeroes";
        public const string ON_BUTTON_CITIES = "ON_btnCities";

        //блок кнопок отряда
        public const string ON_BUTTON_ATTACK = "ON_btnAttack";
        public const string ON_BUTTON_BUILD = "ON_btnBuild";
        public const string ON_BUTTON_DEFFEND = "ON_btnDefffend";
        public const string ON_BUTTON_RECTRUIT = "ON_btnRecrut";
        public const string ON_BUTTON_MOVE = "ON_btnMove";
        public const string ON_BUTTON_MARCH = "ON_btnMarch";
        public const string ON_BUTTON_SKILL1 = "ON_btnSkill1";
        public const string ON_BUTTON_SKILL2 = "ON_btnSkill2";
        public const string ON_BUTTON_SKILL3 = "ON_btnSkill3";

        //блок кнопок миникарты
        //btnGrid не использует Messenger
        public const string ON_BUTTON_GRID = "ON_btnGrid";
        public const string ON_BUTTON_RES = "ON_btnRes";
        public const string ON_BUTTON_VIEW = "ON_btnView";
        public const string ON_BUTTON_CENTER = "ON_btnCenter";
        public const string ON_BUTTON_PLUS = "ON_btnPlus";
        public const string ON_BUTTON_MINUS = "ON_btnMinus";


        //вызов конца хода
        public const string ON_BUTTON_STEP = "ON_btnStep";


        //_________________
        //Игра отпраявляет сообщения-команды, GUI реагирует
        //можно было на изменение написать, но я решил, так будет "отказоустойчивее"
        public const string BUTTON_STEP_SET_ACTIVE = "BUTTON_STEP_SET_ACTIVE";

        public const string SET_AVATAR = "SET_AVATAR";

        public const string UPDATE_RESOURCES = "UPDATE_RESOURCES";

        public const string LOAD_STORY = "LOAD_STORY";

        //public const string FOOD_CHANGED = "FOOD_CHANGED";

        //public const string SPEED_CHANGED = "SPEED_CHANGED";


        //__________


        public const string CREATE_MINIMAP = "CREATE_MINIMAP";
        public const string UPDATE_MINIMAP = "UPDATE_MINIMAP";
    }

    /// <summary>
    /// Справочник событий игрового движка
    /// </summary>
    public class GameEngineEvent
    {
        //*** Обновление данных о ресурсах и их приросте *** 
        public class ResourcesEvent
        {
            //городские Artty: можно выпиливать
            public const string MONEY_INCOME_RESOURCE_CHANGED = "MONEY_INCOME_RESOURCE_CHANGED";
            public const string FOOD_RESOURCE_CHANGED = "FOOD_RESOURCE_CHANGED";
            public const string MATERIALS_RESOURCE_CHANGED = "MATERIALS_RESOURCE_CHANGED";
            public const string POPULATION_RESOURCE_CHANGED = "POPULATION_RESOURCE_CHANGED";

            //бонусные
            public const string HERBS_RESOURCE_CHANGED = "HERBS_RESOURCE_CHANGED";
            public const string CATTLE_RESOURCE_CHANGED = "CATTLE_RESOURCE_CHANGED";
            public const string SOIL_RESOURCE_CHANGED = "SOIL_RESOURCE_CHANGED";
            public const string GOLD_RESOURCE_CHANGED = "GOLD_RESOURCE_CHANGED";
            public const string SILVER_RESOURCE_CHANGED = "SILVER_RESOURCE_CHANGED";
            public const string COPPER_RESOURCE_CHANGED = "COPPER_RESOURCE_CHANGED";

            //глобальные для фракции Artty: можно выпиливать
            public const string MONEY_RESOURCE_CHANGED = "MONEY_RESOURCE_CHANGED";
            public const string WOOD_RESOURCE_CHANGED = "WOOD_RESOURCE_CHANGED";
            public const string STONE_RESOURCE_CHANGED = "STONE_RESOURCE_CHANGED";
            public const string IRON_RESOURCE_CHANGED = "IRON_RESOURCE_CHANGED";
            public const string HIDE_RESOURCE_CHANGED = "HIDE_RESOURCE_CHANGED";
            public const string CATTLES_RESOURCE_CHANGED = "CATTLES_RESOURCE_CHANGED";
            public const string POPULATION_SUMMARY_RESOURCE_CHANGED = "POPULATION_SUMMARY_RESOURCE_CHANGED";
            public const string HORSE_RESOURCE_CHANGED = "HORSE_RESOURCE_CHANGED";
            public const string MANASTONE_RESOURCE_CHANGED = "MANASTONE_RESOURCE_CHANGED";
            public const string MITHRIL_RESOURCE_CHANGED = "MITHRIL_RESOURCE_CHANGED";
            public const string CHARCOAL_RESOURCE_CHANGED = "CHARCOAL_RESOURCE_CHANGED";
            public const string FACTION_RESOURCE_CHANGED = "FACTION_RESOURCE_CHANGED";
        }

        //*** Эвенты отряда ***
        public class SquadEvent
        {
            /// <summary>
            /// Просто пустой эвент, который никогда не случается
            /// </summary>
            public const string Nothing = "SQUAD_Nothing";
            /// <summary>
            /// Отряд подготавливается к движению (повороты, перестановки внутри тайла)
            /// </summary>
            public const string PrepearingForStart = "SQUAD_PrepearingForStart";
            /// <summary>
            /// Отряд начал перемещение по своему ходу (отправляется один раз за перемещение)
            /// </summary>
            public const string StartedMoving = "SQUAD_StartedMoving";
            /// <summary>
            /// Лидер отряда достиг финиша
            /// </summary>
            public const string LiderReachFinish = "SQUAD_LiderReachFinish";
            /// <summary>
            /// Отряд закончил перемещение по ходу     (один раз в конце перемещения)
            /// </summary>
            public const string EndedMoving = "SQUAD_EndedMoving";
            /// <summary>
            /// Отряд был выбран игроком
            /// </summary>
            public const string GotSelected = "SQUAD_GotSelected";
            /// <summary>
            /// Отряд больше не выделен игроком
            /// </summary>
            public const string GotUnselected = "SQUAD_GotUnselected";
            /// <summary>
            /// Отряду назначили путь (перемещение еще не обязательно началось)
            /// </summary>
            public const string PathSet = "SQUAD_PathSet";
            /// <summary>
            /// Отряд не может пройти дальше
            /// </summary>
            public const string NextTurnDenied = "SQUAD_NextTurnDenied";
            /// <summary>
            /// Отряд погиб
            /// </summary>
            public const string Died = "SQUAD_Died";
            /// <summary>
            /// Отряд начал перемещение в следующий тайл пути (вызывается каждый раз для очередного тайла пути)
            /// </summary>
            public const string StartedMovingNextTile = "SQUAD_StartedMovingNextTile";
            /// <summary>
            /// Отряд закончил перемещение в следующий тайл пути (вызывается каждый раз для очередного тайла пути)
            /// </summary>
            public const string NextTilePassed = "SQUAD_NextTilePassed";
            /// <summary>
            /// Отряд остановлен
            /// </summary>
            public const string StopBeforeBattle = "SQUAD_StopBeforeBattle";
            /// <summary>
            /// Одид из юнитов умирает
            /// </summary>
            public const string unitDeath = "SQUAD_unitDeath";
            /// <summary>
            /// Уничтожение отряда
            /// </summary>
            public const string destoy = "SQUAD_destoy";

        }
    }


}
