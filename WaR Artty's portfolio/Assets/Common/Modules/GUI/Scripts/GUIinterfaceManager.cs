using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.GameEngine;

public class GUIinterfaceManager : MonoBehaviour
{
    public CityView cityView;
    public GameView gameView;

	// Use this for initialization
	void Start ()
    {
        Messenger<City>.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, cityView.ShowWindow);
        Messenger<City>.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, gameView.HideWindow);
        Messenger<City>.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, OnCityMode);

        Messenger.AddListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, cityView.HideWindow);
        Messenger.AddListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, gameView.ShowWindow);
        Messenger.AddListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, OffCityMode);
        //Messenger.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, gameView.ShowWindow);
    }

    private void OnDestroy()
    {
        Messenger<City>.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, cityView.ShowWindow);
        Messenger<City>.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, gameView.HideWindow);
        Messenger<City>.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_CITIES, OnCityMode);

        Messenger.RemoveListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, cityView.HideWindow);
        Messenger.RemoveListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, gameView.ShowWindow);
        Messenger.RemoveListener(GameEvents.CityGUIGameEvent.ON_BUTTON_CLOSE, OffCityMode);
    }

    //Обработка события открытия окна города
    private void OnCityMode(City city)
    {
        Camera.main.cullingMask = Camera.main.cullingMask - (1 << LayerMask.NameToLayer("icons"));
        CameraBehaviour.GetThis().SetCameraMode(CameraMode.CityView, city);
    }

    //Обработка события закрытия окна города
    private void OffCityMode()
    {
        Camera.main.cullingMask = Camera.main.cullingMask + (1 << LayerMask.NameToLayer("icons"));
        CameraBehaviour.GetThis().SetCameraMode(CameraMode.GameView);
    }
}
