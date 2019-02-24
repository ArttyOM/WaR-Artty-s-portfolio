using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBTypes;

namespace WAR
{
    /// <summary>
    /// Класс утилит для SBuild
    /// </summary>
    public class SBuildUtils
    {
        private static DataService dataService = DataService.WARSQL;

        /// <summary>
        /// Получение SBuild по его внутреннему имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SBuilding GetSBuildByName(string name)
        {
            foreach (SBuilding sBuilding in dataService.GetBuildings())
            {
                if (sBuilding.InternalName == name)
                {
                    return sBuilding;
                }
            }

            return new SBuilding();
        }

        /// <summary>
        /// Получение BuildingProduction по его ID SBuild'а
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BuildingProduction GetProdutionBySBuildID(int id)
        {
            foreach (SBuilding sBuilding in dataService.GetBuildings())
            {
                if (sBuilding.ID == id)
                {
                    return GetProdutionByID(sBuilding.buildingProductionID);
                }
            }

            return new BuildingProduction();
        }

        /// <summary>
        /// Получение BuildingProduction по его ID
        /// </summary>        
        public static BuildingProduction GetProdutionByID(int id)
        {
            foreach (BuildingProduction buildingProduction in dataService.GetBuildingProductions())
            {
                if (buildingProduction.ID == id)
                {
                    return buildingProduction;
                }
            }

            return new BuildingProduction();
        }

        /// <summary>
        /// Список имен зданий в базе
        /// </summary>
        public static class BuildingNames
        {
            /// <summary>
            /// 0-й технологический уровень
            /// </summary>
            public static class Level0
            {
                public const string TowenHall_Midle = "townhall_midle_lvl0";
                public const string HuntCamp_Midle = "huntcamp_midle_lvl0";
                public const string HuntCamp_North = "huntcamp_north_lvl0";
                public const string HuntCamp_North_Snow = "huntcamp_north_snow_lvl0";
                public const string HuntCamp_South = "huntcamp_north_lvl0";
                public const string ResidentialArea_South = "ResidentialArea_sand_lvl0";
                public const string ResidentialArea_North = "ResidentialArea_north_lvl0";
            }

            /// <summary>
            /// 1-й технологический уровень
            /// </summary>
            public static class Level1
            {                
                public const string ResidentialArea_South = "ResidentialArea_sand_lvl1";
                public const string ResidentialArea_North = "ResidentialArea_north_lvl1";
            }
        }
    }
}
