using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof (PlayerManager))]
//[RequireComponent(typeof (InventoryManager))]

[RequireComponent(typeof (GUIManager))]
[RequireComponent(typeof(CityGUIManager))]

public class Managers : MonoBehaviour {

	//public static PlayerManager Player { get; private set; }
	//public static InventoryManager Inventory { get; private set; }
	//public static WeatherManager Weather { get; private set; }
	//public static ImagesManager Images { get; private set; }

    public static GUIManager GUIM { get; private set; }
    public static CityGUIManager CityGUIM { get; private set; }

    private List<IGameManager> _startSequence;

	void Awake(){
        //Player = GetComponent<PlayerManager>();
        //Inventory = GetComponent<InventoryManager>();
        //Weather = GetComponent<WeatherManager>();
        //Images = GetComponent <ImagesManager>();
        GUIM = GetComponent<GUIManager>();
        CityGUIM = GetComponent<CityGUIManager>();
        _startSequence = new List<IGameManager>();

        _startSequence.Add(GUIM);
        _startSequence.Add(CityGUIM);
		//_startSequence.Add(Player);
		//_startSequence.Add(Inventory);
		//_startSequence.Add(Weather);
		//_startSequence.Add (Images);

		StartCoroutine(StartupManagers());
	}

	private IEnumerator StartupManagers(){
		//NetworkService network = new NetworkService ();

		foreach (IGameManager manager in _startSequence) {
			manager.Startup ();
		}
		yield return null; //остановка на один кадр

		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules) {

			int lastReady = numReady;
			numReady = 0;

			foreach (IGameManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}

			if (numReady > lastReady)
				Debug.Log ("Progress: " + numReady + "/" + numModules);
			yield return null; //остановка на один кадор перед следующей проверкой
		}
		Debug.Log ("All managers started up");
	}
}
