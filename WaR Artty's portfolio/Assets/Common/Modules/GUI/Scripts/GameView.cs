using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using WAR.GameEngine;

public class GameView : MonoBehaviour
{
    public GameObject cityView;
    public Image unitImg;

    //public Sprite unit0;
    //public Sprite unit1;
    //public Sprite unit2;

    //Artty: весь GUI теперь находится под контролем GUI-менеджера
    //public void SetUnitSprite(int i = 0)
    //{
    //    if (i == 0)
    //        unitImg.sprite = unit0;

    //    if (i == 1)
    //        unitImg.sprite = unit1;

    //    if (i == 2)
    //        unitImg.sprite = unit2;
    //}


    private void Awake()
    {
        Messenger.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_GRID, ShowGrid);
        Messenger.AddListener(GameEvents.GUIGameEvent.ON_BUTTON_RES, ShowResources);
    }
    private void OnDestroy()
    {
        
            Messenger.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_GRID, ShowGrid);
            Messenger.RemoveListener(GameEvents.GUIGameEvent.ON_BUTTON_RES, ShowResources);
        
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ShowFog();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShowBorders();
        }
    }

    public void ShowCityView(string name)
    {
        //cityView.GetComponent<CityView>().cityName.text = name;
        //cityView.SetActive(true);
        //this.gameObject.SetActive(false);
    }

    public void ShowResources()
    {
        GenSettings.showbaseresources = !GenSettings.showbaseresources;
    }

    public void ShowGrid()
    {
        int grid = Shader.GetGlobalInt("showgrid");
        Shader.SetGlobalInt("showgrid", Mathf.Abs(1 - grid));
    }

    public void ShowFog()
    {
        int fog = Shader.GetGlobalInt("showfog");
        Shader.SetGlobalInt("showfog", Mathf.Abs(1 - fog));
    }

    public void ShowBorders()
    {
        int borders = Shader.GetGlobalInt("showborders");
        Shader.SetGlobalInt("showborders", Mathf.Abs(1 - borders));
    }

    public void ShowWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void HideWindow(City city)
    {
        this.gameObject.SetActive(false);
    }
}
