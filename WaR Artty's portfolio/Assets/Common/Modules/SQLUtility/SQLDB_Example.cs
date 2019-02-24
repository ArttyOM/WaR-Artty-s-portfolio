using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Data;
using Mono.Data.Sqlite;
using DBTypes;
/// <summary>
/// Пример чтения данных с базы данных SQL
/// </summary>
public class SQLDB_Example : MonoBehaviour 
{

    private void Start()
    {
        //получаем путь к базе данных (для разных платформ разный)
        string connectionString = SQLUtility.DefaultDBPath();
        // эквивалентно string connectionString = SQLUtility.PathToDB("WARSQL");

        //пример упрощенного запроса к БД
        DataService ds = DataService.WARSQL;
        IEnumerable<SBuilding> buildings = ds.GetBuildings();

        foreach (SBuilding building in buildings)
        {
            Debug.Log(building.ToString());

            //ToConsole(person.ToString());
        }
        

        //пример прямого запроса к БД
        /*
        using (IDbConnection dbcon = (IDbConnection)new SqliteConnection(connectionString))
        {
            dbcon.Open();
            // Выбираем нужные нам данные
            var sql = "SELECT ID, pathToImage, productionCost FROM SBuilding";
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

                        const string frmt = "id: {0}; MainImagePath: {1}; productionCost: {2};";
                        Debug.Log(string.Format(frmt,
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2)
                          ));
                    }
                }
            }
            // Закрываем соединение
            dbcon.Close();
        }

        */
    }
}