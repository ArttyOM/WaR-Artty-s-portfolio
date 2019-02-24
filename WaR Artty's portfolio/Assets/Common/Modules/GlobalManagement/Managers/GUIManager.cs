using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using WAR.GameEngine;

public class GUIManager : MonoBehaviour, IGameManager
{
    //[Inject]
    //private GodBehaviour _God;

    //TODO написать ограничитель прокрутки при конце файла

    [SerializeField] private RectTransform _stepParentObject;
    private Button _btnEndTurn;//переписать потом под самоопределение
    private Text _thisTurnTextDisplayed;
    [SerializeField] private Image _unitAvatar;

    [SerializeField] private List<Text> _resourceList;

    [SerializeField] private Toggle _toggleShowStory;
    [SerializeField] private Toggle _tmpToggleShowStory;
    [SerializeField] private RectTransform _scrollParentObject;
    private ScrollRect _storyScrollView;
    private Scrollbar _storyVerticalScroll;
    private Text _storyDispayedTXT;
    //словарь возможных значений картинки Аватаров
    //TO DOподумать как переделать
    //в отличае от ресурсов, юнитов может быть достаточно много, поэтому сериализация через Unity идея так себе
    private Dictionary<int, Sprite> _unitAvatarsDictionary = new Dictionary<int, Sprite>();

    //для сериализации через Unity структура Dictionary не подходит, используем List и перенаправим в Dictionary
    [SerializeField] private List<Sprite> _unitAvatarsList;

    private int _thisTurn = 0;

    public ManagerStatus status { get; private set; }

    void Awake()
    {
        _btnEndTurn = _stepParentObject.GetComponentInChildren<Button>();
        Transform tmpTransform = _stepParentObject.GetChild(0).GetChild(0);
        if (tmpTransform == null)
        {
            Debug.Log("Не найден объект текста конца хода _stepParentObject.GetChild(0).GetChild(0)");   
        }
        else _thisTurnTextDisplayed = tmpTransform.GetComponent<Text>();

        _storyScrollView = _scrollParentObject.GetComponentInChildren<ScrollRect>();
        _storyVerticalScroll = _storyScrollView.verticalScrollbar;
        _storyDispayedTXT = _storyScrollView.content.gameObject.GetComponent<Text>();
        if (_storyDispayedTXT == null) Debug.Log("Текстовый контент не найден");

        //при получении сообщения "UPDATE_RESOURCES" автоматически выполняется метод UpdateResources(Dictionary<string string имяРесурса, float количествоРесурса>)
        Messenger<Dictionary<string, float>>.AddListener(GameEvents.GUIGameEvent.UPDATE_RESOURCES, UpdateResources);

        // тоже самое для метода с одним параметром
        Messenger<bool>.AddListener(GameEvents.GUIGameEvent.BUTTON_STEP_SET_ACTIVE, SetButtonStepActive);

        // по запросу меняем аватар выбранного отряда/героя
        Messenger<Sprite>.AddListener(GameEvents.GUIGameEvent.SET_AVATAR, SetAvatar);

        // по запросу подгружаем текст согласно адресу 
        Messenger<string>.AddListener(GameEvents.GUIGameEvent.LOAD_STORY, LoadStory);


        int id = 0;
        foreach (Sprite tmpSprite in _unitAvatarsList)
        {
            _unitAvatarsDictionary.Add(id, tmpSprite);
            id++;
        }
        //Debug.Log("_______");
        //Debug.Log("Справочник аватаров:");
        //foreach (KeyValuePair<int, Sprite> tmpSprite in _unitAvatarsDictionary)
        //{
        //    Debug.Log(tmpSprite.Key + " " + tmpSprite.Value.name);
        //}
        //Debug.Log("_______");
    }
    void OnDestroy()
    {
        Messenger<Dictionary<string, float>>.RemoveListener(GameEvents.GUIGameEvent.UPDATE_RESOURCES, UpdateResources);
        Messenger<bool>.RemoveListener(GameEvents.GUIGameEvent.BUTTON_STEP_SET_ACTIVE, SetButtonStepActive);
        Messenger<string>.RemoveListener(GameEvents.GUIGameEvent.LOAD_STORY, LoadStory);

    }
    public void Startup()
    {
        Debug.Log("GUI manager started...");

        status = ManagerStatus.Started;
    }
    /// <summary>
    /// Метод отображает в GUI float-значения  ресурсов (по string-ключу) согласно поданному на вход "справочнику" Resourses 
    /// string-ключ содержит наименование ресурса
    /// float - его количественное значение
    /// </summary>
    /// <param name="Resourses"></param>
    void UpdateResources(Dictionary<string, float> Resources)
    {
        //Debug.Log("сообщение получено");
        //Debug.Log("полученный справочник:");
        //foreach (KeyValuePair<string, float> resource in Resources)
        //{
        //    Debug.Log(resource.Key + " " + resource.Value);
        //}

        //пересчет ресурсов будет происходить вне GUI-системы
        //в GUI же просто считываем данные по ресурсам для их отображения

        //ищем совпадения ключей внешнего справочника со списком ресурсов GUI
        foreach (KeyValuePair<string, float> resource in Resources)
        {
            bool flaq = false;
            foreach (Text textGUI in _resourceList)
            {
                flaq = false;
                if (textGUI.gameObject.name == resource.Key)
                {
                    textGUI.text = resource.Value.ToString();
                    flaq = true;
                    break;//нет смысла бежать по всему оставшемуся списку, если мы уже нашли нужный ресурс
                }
            }
            if (!flaq)
            {
                Debug.Log("Ресурса " + resource.Key + " не существует списке GUI-ресурсов");
            }

        }
    }

    //блок кнопок Units Cities Heroes
    public void OnBtnUnits() //вешать на кнопку btnUnits
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_UNITS);
    }
    public void OnBtnHeroes() //вешать на кнопку btnHeroes
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_HEROES);
    }
    public void OnBtnCities() //вешать на кнопку btnCities
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_CITIES);
    }
    //_____________
    //блок кнопок отряда
    public void OnBtnAttack()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_ATTACK);
    }
    public void OnBtnBuild()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_BUILD);
    }
    public void OnBtnDeffend()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_DEFFEND);
    }
    public void OnBtnRecrut()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_RECTRUIT);
    }
    public void OnBtnMove()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_MOVE);
    }
    public void OnBtnMarch()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_MARCH);
    }

    public void OnBtnSkill1()
    {
        Messenger<Squad>.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_SKILL1, _God.selectedSquad);
    }

    public void OnBtnSkill2()
    {
        Messenger<Squad>.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_SKILL2, _God.selectedSquad);
    }

    public void OnBtnSkill3()
    {
        Messenger<Squad>.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_SKILL3, _God.selectedSquad);
    }

    //___________

    public void OnBtnStep()
    {
        if (_btnEndTurn == null)
        {
            Debug.Log("WARNING!Кнопка конца хода не назначена");
            //_btnEndTurn =
        }
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_STEP);


    } //вешать на кнопку конца хода

    public void SetButtonStepActive(bool isActive)
    {
        _btnEndTurn.gameObject.SetActive(isActive);

        if (isActive) // если кнопку снова сделали активной, значит предыдущий ход закончен
        {
            _thisTurn++;
            _thisTurnTextDisplayed.text = _thisTurn.ToString();

            _God.turn = _thisTurn;
        }
    }

    public void SetAvatar(Sprite sprite)
    {
        if (sprite == null)
        {
            _unitAvatar.color = new Color(1, 1, 1, 0);
        }
        else
            _unitAvatar.color = new Color(1, 1, 1, 1);

        _unitAvatar.sprite = sprite;
        /* bool flaq = false;
         foreach (KeyValuePair<int, Sprite> tmpAvatar in _unitAvatarsDictionary)
         {
             if (avatarID == tmpAvatar.Key)
             {
                 _unitAvatar.sprite = tmpAvatar.Value;
                 flaq = true;
                 break;
             }
         }
         if (!flaq)
         {
             Debug.Log("Спрайт с ключем " + avatarID + " не обнаружен в справочнике GUI-ресурсов");
         }
         */
    }

    //костыль-заглушка для теста всплывающего окна
    /// <summary>
    /// Кликая на временную кнопку-костыль, 
    /// подгружается текст, делается видимым,
    /// передается управление нормальной кнопке. 
    /// Костыль при этом становится неактивным, т.к. больше не нужен
    /// </summary>
    public void OnTempToggleShowStoryValueChanged()
    {
        //Debug.Log(_toggleShowStory.isOn);

        //LoadStory("texts/test1");//в рамках GUI эта строчка похдит лучше
        //но для демонстрации как функцию LoadStory использовать из других скриптов сделал через Broadcast
        Messenger<string>.Broadcast(GameEvents.GUIGameEvent.LOAD_STORY, "texts/test1");
        
        // грузим текст и делаем его видимым
        _toggleShowStory.gameObject.SetActive(true); // делаем видимым номарльную кнопку
        _tmpToggleShowStory.gameObject.SetActive(false); //делаем невидимой кнопку-костыль



    }

    public void OnToggleShowStoryValueChanged()
    {
        //Debug.Log(_toggleShowStory.isOn);
        ShowStory(_toggleShowStory.isOn);
    }

    //TODO написать ограничитель прокрутки при конце файла
    public void LoadStory(string pathToTXT) //в параметрах указать ссылку на сам текст
    {
        TextAsset importedTXT  =  (TextAsset) Resources.Load(pathToTXT);
        _storyDispayedTXT.text = importedTXT.text;
        _scrollParentObject.gameObject.SetActive(true);
        //Debug.Log(_StoryScrollView.gameObject.active);
        //на всякий случай установим ползунок скроллера на начальную позицию
        _storyScrollView.verticalScrollbar.value = 1f;
    }

    public void ShowStory(bool isActive)
    {
        //на всякий случай установим ползунок скроллера на начальную позицию
        _storyScrollView.verticalScrollbar.value = 1f;

        _scrollParentObject.gameObject.SetActive(isActive);
    }

    public void OnBtnOfVerticalScroller(float deltaValue)
    { 
        //индусский код - алилуя
        if (_storyVerticalScroll.value + deltaValue > 1) _storyVerticalScroll.value = 1;
        else if (_storyVerticalScroll.value + deltaValue < 0) _storyVerticalScroll.value = 0;
        else _storyVerticalScroll.value += deltaValue;
    }

    //____________
    //Группа кнопок миникарты
    //перенесено из GameView 
    public void OnBtnGrid()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_GRID);
    }

    //перенесено из GameView 
    public void OnBtnRes()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_RES);
    }

    public void OnBtnView()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_VIEW);
    }
    public void OnBtnCenter()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_CENTER);
    }
    public void OnBtnPlus()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_PLUS);
    }
    public void OnBtnMinus()
    {
        Messenger.Broadcast(GameEvents.GUIGameEvent.ON_BUTTON_MINUS);
    }


}
