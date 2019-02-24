namespace WAR
{

    /// <summary>
    /// Класс хранения последнего ID и генерации следующего
    /// </summary>
    public static class IdStorage
    {
        //фракции
        static private int _FractionsID = -1;

        //города
        static private int _CityesID = -1;

        //отряды
        static private int _SquadsID = -1;

        /// <summary>
        /// Получение ID для фракции
        /// </summary>    
        public static int GetFractionId()
        {
            _FractionsID++;

            return _FractionsID;
        }

        /// <summary>
        /// Получение ID для города
        /// </summary>    
        public static int GetCityId()
        {
            _CityesID++;

            return _CityesID;
        }

        /// <summary>
        /// Получение ID для отряда
        /// </summary>    
        public static int GetSquadId()
        {
            _SquadsID++;

            return _SquadsID;
        }

    }
}
