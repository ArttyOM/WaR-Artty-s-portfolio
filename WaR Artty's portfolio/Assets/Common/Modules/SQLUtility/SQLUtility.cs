using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

/// <summary>
/// класс содержит методы для получения корректно работающего на разных платформах пути к БД
/// </summary>
public class SQLUtility 
{
    /// <summary>
    /// Используйте в каче
    /// </summary>
    /// <returns>кроссплатформенный</returns>
    public static string DefaultDBPath()
    {
        return PathToDB("WARSQL");
    }

    /// <summary>
    /// TODO: протестировать на IOS, Linux, MacOS
    /// Если нужно получить кроссплатформенный путь к файлу БД "Assets\StreamingAssets\yourBD.byte",
    /// используйте PathToDB("yourBD.byte")
    /// ограничения:
    /// файл БД должен иметь расширение .byte
    /// файл БД должен лежать в  папкеStreamingAssets
    /// </summary>
    /// <param name="nameOfSQLDB">имя базы данных в папке StreamingAssets, без указания типа файла, например, "WARSQL"</param>
    /// <returns>в зависимости от используемой платформы возвращается разный путь к ДБ, требуется для кроссплатформенности
    /// </returns>
    public static string PathToDB(string nameOfSQLDB)
    {
        string connectionString = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, nameOfSQLDB+".byte");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { } // TODO: добавить таймер

            string realPath = Application.persistentDataPath + "/"+ nameOfSQLDB;
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            connectionString = realPath;
        }
        else
        {
            // iOS + Windows
            connectionString = System.IO.Path.Combine(Application.streamingAssetsPath, nameOfSQLDB+".byte");
        }
        string prefix = "URI=file:"; //дефолтное значение для windows
        //Debug.Log(connectionString);
        connectionString = prefix + connectionString;
        return connectionString;
    }
}
    
