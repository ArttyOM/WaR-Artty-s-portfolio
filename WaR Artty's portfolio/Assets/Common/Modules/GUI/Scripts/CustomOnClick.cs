using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// С параметром Auto кнопка будет передавать в качестве сообщения свое имя
/// </summary>
public class CustomOnClick : MonoBehaviour {

    //public int armor = 75;
    //public int damage = 25;
    

    [SerializeField]
    private ButtonOwner window = ButtonOwner.Auto;
    [SerializeField]
    private GameEvents1.UIGameEvent onGameViewEvent;
    [SerializeField]
    private GameEvents1.UICityEvent onCityViewEvent;
    //private GameEvents

    private void Awake()
    {

        Enum myGameEvent = onGameViewEvent;

        switch (window)
        {
            case ButtonOwner.Auto:
                //Debug.Log(name);

                //пытаемся преобразовать имя нашего Gameobject-а в Enum
                GameEvents1.UIGameEvent tmpGameEvent;
                GameEvents1.UICityEvent tmpCityEvent;
                
                if (TryParse<GameEvents1.UIGameEvent>("ON_" + name, out tmpGameEvent))
                {
                    //Если получается преобразовать в UIGameEvent - кешируем в myGameEvent, потом отдадим его в OnBtn
                    myGameEvent = tmpGameEvent;
                }
                else//если не получается, пробуем преобразовать в UICityEvent
                {
                    if (TryParse<GameEvents1.UICityEvent>("ON_" + name, out tmpCityEvent))
                    {
                        //Если получается преобразовать в UICityEvent - кешируем в myGameEvent, потом отдадим его в OnBtn
                        myGameEvent = tmpCityEvent;
                    }
                    else return;//если опять не получилось -> не является нашим энум, не вешаем на кнопку
                }

                break;
            case ButtonOwner.GameView:
                myGameEvent = onGameViewEvent;
                break;
            case ButtonOwner.CityView:
                myGameEvent = onCityViewEvent;
                break;
            default: break;
        }

        GetComponent<Button>().onClick.AddListener(delegate { OnBtn(myGameEvent); });
    }

    private void OnBtn (Enum onClickEvent)
    {
        Debug.Log(onClickEvent.ToString());
        Messenger.Broadcast(onClickEvent.ToString());
    }

    /// <summary>
    /// В .Net 3.5 нет Enum.TryParse, пришлось стырить c тырнетов
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse<TEnum>(string value, out TEnum result)
    where TEnum : struct, IConvertible
    {
        var retValue = value == null ?
                    false :
                    Enum.IsDefined(typeof(TEnum), value);
        result = retValue ?
                    (TEnum)Enum.Parse(typeof(TEnum), value) :
                    default(TEnum);
        return retValue;
    }

}



/// <summary>
/// Режим Авто передает в качестве сообщения по клику ("ON_"+имя объекта)
/// </summary>
public enum ButtonOwner { Auto, GameView, CityView }
