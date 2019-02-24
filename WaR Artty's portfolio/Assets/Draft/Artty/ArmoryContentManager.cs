using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;


namespace WAR
{
    public class ArmoryContentManager : MonoBehaviour
    {

        [SerializeField] private Transform _content;
        [SerializeField] private Dropdown _dropdown;
        [SerializeField] private GameObject _pref;
        //в соответствии с настройками DropDown (порядок соблюден)
        

        private static List<Outfit> outfits = new List<Outfit>();

        //public void OnDropDownValueChanged()
        //{
        //    switch (_dropdown.value)
        //    {
        //        case (int)EquipmentType.Weapon: break;
        //        case (int)EquipmentType.OffHand: break;
        //        case (int)EquipmentType.Helm: break;
        //        case (int)EquipmentType.Armor: break;
        //        case (int)EquipmentType.Mount: break;
        //        default: break;

        //    }
        //}

        
        private void Awake()
        {
            //получаем путь к базе данных (для разных платформ разный)
            string connectionString = SQLUtility.DefaultDBPath();
            // эквивалентно string connectionString = SQLUtility.PathToDB("WARSQL");

            using (IDbConnection dbcon = (IDbConnection)new SqliteConnection(connectionString))
            {
                dbcon.Open();
                // Выбираем нужные нам данные
                var sql = "SELECT id, name, type, imagePath, userDescription FROM Armory";
                using (IDbCommand dbcmd = dbcon.CreateCommand())
                {
                    dbcmd.CommandText = sql;
                    // Выполняем запрос
                    using (IDataReader reader = dbcmd.ExecuteReader())
                    {
                        // Читаем и выводим результат
                        while (reader.Read())
                        {
                            //место для вашего кода
                            Outfit outfit;
                            outfit.id = reader.GetInt32(0);
                            outfit.name = reader.GetString(1);
                            outfit.type = reader.GetString(2);
                            outfit.imagePath = reader.GetString(3);
                            outfit.userDescription = reader.GetString(4);
                            GameObject tmp = Instantiate(_pref, _content);
                            Equipment.CreateComponent(tmp, outfit);

                            //const string frmt = "id: {0}; MainImagePath: {1}; type: {2};";
                            //Debug.Log(string.Format(frmt,
                            //    reader.GetInt32(0),
                            //    reader.GetString(3),
                            //    reader.GetString(2)
                            //  ));
                        }
                    }
                }
                // Закрываем соединение
                dbcon.Close();
            }
            
        }
}
    
    public struct Outfit
    {
        public int id;
        public string name;
        public string type;
        public string imagePath;
        public string userDescription;

    }
    public enum EquipmentType {no_set,weapon, offhand, helm, armor, mount }
}
