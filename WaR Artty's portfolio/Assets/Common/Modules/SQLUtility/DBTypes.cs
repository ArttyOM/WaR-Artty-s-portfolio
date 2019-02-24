using SQLite4Unity3d;
using System.Collections.Generic;
using UnityEngine;
using WAR.GameEngine;

namespace DBTypes
{
    /// <summary>
    /// Структура для вычитки технологий из таблицы Tech
    /// </summary>
    public struct Tech
    {
        //свойства, подтягивающиеся с SQL-БД
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int Tier { set; get; }
        public int TechTree { set; get; }
        public int isNode { set; get; }
        public string PathToPicture { set; get; }
        public int Cost { set; get; }
    }

    /// <summary>
    /// Структура для вычитки технологических деревьев из таблицы TechTrees
    /// </summary>
    public struct TechTrees
    {
        //свойства, подтягивающиеся с SQL-БД
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int ID { set; get; }
        public string Name { set; get; }
    }

    /// <summary>
    /// Структура для вычитки зависимостей технологий из таблицы TechDependence
    /// </summary>
    public struct TechDependence
    {
        //свойства, подтягивающиеся с SQL-БД
        public int ID { set; get; }
        public int ID_Dependence { set; get; }
        public int isOptional { set; get; }
    }

    /// <summary>
    /// Структура для вычитки навыков
    /// </summary>
    public struct Skills
    {
        //свойства, подтягивающиеся с SQL-БД
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int Range { set; get; }
        public int Duration { set; get; }
        public string PathToPrefab { set; get; }
        public string PathToImage { set; get; }
    }

    /*
    /// <summary>
    /// Структура для вычитки навыков
    /// </summary>
    public struct District
    {
        //свойства, подтягивающиеся с SQL-БД
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }        
    }

    /// <summary>
    /// Структура для вычитки навыков
    /// </summary>
    public struct DistrictProduction
    {
        //свойства, подтягивающиеся с SQL-БД
        public int ID { set; get; }
        public int Materials { set; get; }
        public int Food { set; get; }
        public int Money { set; get; }
    }
    */

    /// <summary>
    /// Объект, содержащий всю необходимую информацию о Постройке
    /// </summary>
    public struct SBuilding
    {
        //свойства, подтягивающиеся с SQL-БД
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string pathToImage { get; set; }//путь к спрайту здания
        public int buybackCost { get; set; }//стоимость постройки за деньги (моментально) 
        public int productionCost { get; set; }//стоимость постройки за производство
        //public int upkeep { get; set; }//базовое содержание постройки (перенес в BuildingProduction)
        public string displayedDesctription { get; set; }//отображаемое игроку описание 
        //TODO перенести displayedDesctription  в другую табличку
        public string descript { get; set; }//свойство с описанием для внутреннего пользования
        public string PathConstractionSite { get; set; }
        public int BuildingTypeID{get;set;} //Ключ к таблице с типом здания 
        public int districtOwnerID{get; set;} //ID района-владельца
        public int buildingProductionID { get; set; } //ID производства базовых ресурсов от  здания
        public int BiomType { get; set; } //ID типа биома, которому принадлежит здание
        public int TechLevel { get; set; } //Уровень технологии этого здания
        public string InternalName { get; set; } //внутреннее имя для кода
        //_______

        //свойства для экземпляров структуры без использования БД
        public City buildingOwner { get; private set; }
        public int idCityOwner { get; private set; }
        public Status status { get; private set; }

        //_______

        public void SetStatus(Status status)
        {
            this.status = status;
        }

        public void SetOwner(City newOwner)
        {
            this.buildingOwner = newOwner;
        }

        public override string ToString()
        {
            return string.Format("[Owner={6} , OwnerID= {7} ,Building: Id={0}, pathToImage={1}, buybackCost={2}, productionCost={3}, displayedDesctription={4}, descript = {5}]",
                ID, pathToImage, buybackCost, productionCost, displayedDesctription, descript, buildingOwner, idCityOwner);
        }

    }

    //свойства построенного здания
    public struct BuildingProduction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public float Upkeep { get; set; } //содержание здания (деньги)
        public float MoneyIncome { get; set; } //прибыль здания (деньги)
        public float FoodIncome { get; set; } //прибыль здания (еда)
        public float MaterialIncome { get; set; } //прибыль здания (материалы)
    }


    public struct Unit
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PathToImage { get; set; }//путь к спрайту юнита
        public string PathToAva { get; set; }//путь к аватару юнита
        public string PathToPrefab { get; set; }
        public int BuybackCost { get; set; }//стоимость найма за деньги (моментально) 
        public int ProductionCost { get; set; }//базовая стоимость постройки за производство

        public float MoneyUpkeep { get; set; }//базовое содержание 1го юнита (в отряде 9)
        public float FoodUpkeep { get; set; }//базовое потребление пищи 1го юнита (в отряде 9)
        public float FoodPool { get; set; } //запас пищи, который может хранить 1 юнит

        public string Descript { get; set; }//свойство с описанием для внутреннего пользования

        //свойства для экземпляров структуры без использования БД
        //public PlayerController Owner {get; set;}//Хозяин юнита
        public Status status { get; private set; }

        //TODO актуализировать метод
        public override string ToString()
        {
            return string.Format("[Unit: Id={0}, pathToImage={1}, buybackCost={2}, productionCost={3}, upkeep={4}, descript = {5}]",
                ID, PathToImage, BuybackCost, ProductionCost, MoneyUpkeep, Descript);
        }

    }

    /// <summary>
    /// Статусы, помогающие реализовать жизненный цикл
    /// Построек(Building) и Войск(Units)
    /// </summary>
    public enum Status
    {
        New = 0,//только что созданный экземпляр 
        Avaiable,//доступный к постройке
        NotAvaiable,//не доступный (например, не хватает спец. ресурса)
        InProgress,//строится обычным способом
        InQueue,//готов к постройке обычным способом, ждет своей очереди
        Paid,//оплачен, в следующий ход будет построен
        ToDestroy//подготовка к уничтожению

    }

}

