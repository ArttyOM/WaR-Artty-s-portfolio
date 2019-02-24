using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// синглтон справочник построек
/// </summary>


public class BuildingsDictionary
{
    private static object syncRoot = new System.Object();

    private static BuildingsDictionary instance;

    //private BuildingsDictionary() { }

    public static BuildingsDictionary GetInstance()
    {
        if (instance == null)
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new BuildingsDictionary();
                    
                }
            }
        return instance;
    }





    //справочник типов зданий
    //ключ, ходов на постройку,содержание , стоимость(выкуп), эффекты от здания, стоимость(ресурсы)


    //public List<Building> dict = new List<Building>(); //если зданий будет очень много, нужно будет подобрать более подходящую структуру
}
