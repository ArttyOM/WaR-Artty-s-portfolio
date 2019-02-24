using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using DBTypes;
public class DataService  {

    private static DataService _serviceWARSQL;
    public static DataService WARSQL
    {
        get
        {
            if (_serviceWARSQL == null) _serviceWARSQL = new DataService("WARSQL.byte");
            return _serviceWARSQL;
        }
        private set
        {
            _serviceWARSQL = value;
        }
    }


    private SQLiteConnection _connection;

	public DataService(string DatabaseName = "WARSQL.byte")
    {

#if UNITY_EDITOR
            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.streamingAssetsPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log("Final PATH: " + dbPath);     

	}

    public IEnumerable<SBuilding> GetBuildings()
    {
        return _connection.Table<SBuilding>();
    }

    public IEnumerable<SBuilding> GetBuildingsWithImage()
    {
        return _connection.Table<SBuilding>().Where(x => (x.pathToImage!=null||x.pathToImage!=""));
    }

    public IEnumerable<DBTypes.Unit> GetUnits()
    {
        return _connection.Table<DBTypes.Unit>();
    }

    /// <summary>
    /// Получение всех технологий
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.Tech> GetTechs()
    {
        return _connection.Table<DBTypes.Tech>();
    }

    /// <summary>
    /// Получение технологических деревьев
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.TechTrees> GetTechTrees()
    {
        return _connection.Table<DBTypes.TechTrees>();
    }

    /// <summary>
    /// Получение зависимостей технологий
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.TechDependence> GetTechDependence()
    {
        return _connection.Table<DBTypes.TechDependence>();
    }
/*
    /// <summary>
    /// Получение районов
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.District> GetDistricts()
    {
        return _connection.Table<DBTypes.District>();
    }

    /// <summary>
    /// Получение доходов районов
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.DistrictProduction> GetDistrictsProduction()
    {
        return _connection.Table<DBTypes.DistrictProduction>();
    }
*/
    /// <summary>
    /// Получение доходов зданий
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.BuildingProduction> GetBuildingProductions()
    {
        return _connection.Table<DBTypes.BuildingProduction>();
    }

    /// <summary>
    /// Получение навыков
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DBTypes.Skills> GetSkills()
    {
        return _connection.Table<DBTypes.Skills>();
    }

    /*
        public void CreateDB(){
            _connection.DropTable<Person> ();
            _connection.CreateTable<Person> ();

            _connection.InsertAll (new[]{
                new Person{
                    Id = 1,
                    Name = "Tom",
                    Surname = "Perez",
                    Age = 56
                },
                new Person{
                    Id = 2,
                    Name = "Fred",
                    Surname = "Arthurson",
                    Age = 16
                },
                new Person{
                    Id = 3,
                    Name = "John",
                    Surname = "Doe",
                    Age = 25
                },
                new Person{
                    Id = 4,
                    Name = "Roberto",
                    Surname = "Huertas",
                    Age = 37
                }
            });
        }
        */

    public IEnumerable<Person> GetPersons(){
            return _connection.Table<Person>();
        }
        
        public IEnumerable<Person> GetPersonsNamedRoberto(){
		return _connection.Table<Person>().Where(x => x.Name == "Roberto");
	}

	public Person GetJohnny(){
		return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
	}

	public Person CreatePerson(){
		var p = new Person{
				Name = "Johnny",
				Surname = "Mnemonic",
				Age = 21
		};
		_connection.Insert (p);
		return p;
	}

    public void CreateBuildingProduction()
    {
        _connection.CreateTable<BuildingProduction>();
    }

    
}
