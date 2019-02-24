using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WAR.GameEngine;
using DBTypes;

public class CreationQueueElement : MonoBehaviour//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Button _btnCancel;
    private Button _btnCoin;
    private Button _btnPause;
    private Button _btnUnpause;
    //private SBuilding building;
    private Slider _progressBar;
    private Text _turnsToComplete;
    private /*BuildingInProgressInfo*/IInProgressInfo _progressInfo;


    private static GameObject _queuePrefab;
    private static GameObject _content;
    //private static GameObject _tmpStub;//Заглушка для плавной анимации перетаскивания
    //private static Animator _stubAnimator; 


    //private GameObject m_DraggingIcon;
    //private RectTransform m_DraggingPlane;

    //___________________________________
    /// <summary>
    /// Фабричный метод
    /// 1) находит нужный префаб и род.объект, куда его цеплять (если соответствующие стат.поля ==null)
    /// 2) создает новый GameObject из префаба, проставляет ему род.объект, привязывает к нему SBuilding из входного параметра
    /// 3) находит или генерит анимированную заглушку для плавного перемещения объектов в очереди (если у класс уже её не хранит)
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static GameObject InstantiateQueueElement(/*BuildingInProgressInfo*/IInProgressInfo info/*SBuilding myBuilding*/)
    {
        //1) находит нужный префаб и куда его цеплять
        if (_queuePrefab == null)
            _queuePrefab = Resources.Load<GameObject>("_default_queueLine");
        if (_content == null)
            _content = CityGUIManager.creationQueue;
            //_content = GameObject.Find("QueueContent");

        //2) создает новый GameObject из префаба, проставляет ему род.объект, привязывает к нему SBuilding из входного параметра
        GameObject result = Instantiate(_queuePrefab, _content.transform);
        CreationQueueElement tmpQueueElement = result.GetComponent<CreationQueueElement>();
        tmpQueueElement._progressBar = result.GetComponentInChildren<Slider>();
        tmpQueueElement._turnsToComplete = result.transform.Find("TurnsToComplete").GetComponent<Text>();
        tmpQueueElement._progressInfo = info;

        tmpQueueElement._progressBar.value = tmpQueueElement._progressInfo.ProgressPercent;
        tmpQueueElement._turnsToComplete.text = tmpQueueElement._progressInfo.StepToComplete.ToString();

        if (tmpQueueElement._progressInfo == null) Debug.Log("Всё плохо");
        else Debug.Log(tmpQueueElement._progressInfo.ToString());
        //tmpQueueElement.building = myBuilding;
        //tmpQueueElement.

        //CreationQueueElement queueElement = where.AddComponent<CreationQueueElement>();

        //3) находит или генерит анимированную заглушку для плавного перемещения объектов в очереди(если у класс уже её не хранит)
        //if (_tmpStub == null)//сначала ищем по иерархии
        //    _tmpStub = GameObject.Find("tmpStub");
        //if (_tmpStub == null)//если Find не нашел объект, создадим ручками из префаба
        //    _tmpStub = Instantiate(Resources.Load<GameObject>("tmpStub"), _content.transform);

        //if (_stubAnimator == null)
        //    _stubAnimator = _tmpStub.GetComponent<Animator>();
        //_tmpStub.transform.SetAsLastSibling();

        return result;
    }

    //_______________________________
    //Стандартные методы Monobehaviour
    public void Awake()
    {
        //_progressBar = GetComponentInChildren<Slider>();
        //        _progressBar.value = _progressInfo.ProgressPercent;
        //_progressBar.value = 0.7f;//просто протестить

        _btnCoin = this.transform.Find("Buyout").GetComponent<Button>();
        _btnCoin.onClick.AddListener(delegate { OnBtnCoin(_progressInfo); });

        _btnCancel = this.transform.Find("Cancel").GetComponent<Button>();
        _btnCancel.onClick.AddListener(delegate { OnBtnCancel(_progressInfo); });

        _btnPause = this.transform.Find("Pause").GetComponent<Button>();
        _btnCancel.onClick.AddListener(delegate { OnBtnPause(_progressInfo); });

        _btnUnpause = this.transform.Find("Unpause").GetComponent<Button>();
        _btnCancel.onClick.AddListener(delegate { OnBtnUnpause(_progressInfo); });

        //Fetch the Raycaster from the GameObject (the Canvas)
        //m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        //m_EventSystem = GetComponentInParent<EventSystem>();

    }

    #region невостребованный код
    //________________________________________
    //Методы, реализующие интерфейсы IBeginDragHandler, IDragHandler, IEndDragHandler
    /*Закомментил за их ненадобностью 
    (постройки строятся одновременно, поэтому не нуждаются в реализации перетасовки очереди)
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    GameObject lastSelectedGObj;
    int siblingIndex=0;
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        //включаем отображение анимированной заглушки (она нужна для плавности вставки в очередь)
        _tmpStub.gameObject.SetActive(true);
        //_stubAnimator.SetBool("IsStubActive", true);

        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;


        //прячем выбранный объект, предварительнозапоминая позицию в текущей иерархии
        siblingIndex = this.transform.GetSiblingIndex();
        Debug.Log(siblingIndex);


        // this.transform.SetParent(transform.root);//TODO: придумать что-то лучше этого костыля
        //склонируем объект, чтобы позволить его перенести и в случае чего моментально откатить
        m_DraggingIcon = this.gameObject;//Instantiate(this.gameObject, canvas.transform);
        m_DraggingIcon.GetComponent<Image>().raycastTarget = false;//чтобы не мешал тянуть сам себя

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        //m_DraggingIcon.transform.SetAsLastSibling();

        //Color color = this.GetComponent<Image>().color;
        //color.a = 0.2f;
    }
    public void OnDrag(PointerEventData data)
    {
        //отображение перетягиваемого объекта тащится за курсором
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);

        //bool isStubActive = false;
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        foreach (RaycastResult result in results)
        {
            
            CreationQueueElement queueResult = result.gameObject.GetComponent<CreationQueueElement>();
            //1) попался объект очереди, отличный от последнего выделенного->воспроизвести анимацию
            if (queueResult != null)
            {
                if (result.gameObject == lastSelectedGObj)
                {
                    
                    _tmpStub.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex());
                    //isStubActive = true;
                    //Debug.Log("yo" + result.gameObject.transform.GetSiblingIndex());
                }
                else
                {
                    lastSelectedGObj = result.gameObject;
                    //isStubActive = false;
                    //если мы смотрим на другой объект, нужно воспроизвести анимацию, для этого её надо сбросить 

                }
                break;
            }
            //2) мы не слишком далеко увели мышку, курсор находится над заглушкой
            if (result.gameObject == _tmpStub)
            {
                //isStubActive = true;
                break;
            }
        }

        //_stubAnimator.SetBool("IsStubActive", isStubActive);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //возвращаем объект в иерархию очереди
        this.transform.SetParent(_content.transform);
        Debug.Log(siblingIndex);
        this.transform.SetSiblingIndex(siblingIndex+1);
        //индекс отличен от начального, т.к. в OnBeginDrag _tmpStub был последним в иерархии

        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == _tmpStub )
            {
                this.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex());
                //Debug.Log("yo" + result.gameObject.transform.GetSiblingIndex());
                break;
            } 
            //окно, с которым мы сравниваем результат райкаста, 
            //находится на ступень выше по иерархии относительно контента
            if (result.gameObject == _content.transform.parent.gameObject)
            {
                //this.transform.SetParent(_content.transform, false);
                this.transform.SetAsLastSibling();
                //this.transform.localPosition = Vector3.zero;
               // Debug.Log("Позиция выставлена");
            }
           // Debug.Log("Hit " + result.gameObject.name);
        }

        //временную заглушку с анимацией прячем до следующего вызова OnDrag
        //_tmpStub.SetActive(false);
        //_stubAnimator.SetBool("IsStubActive", false);
        _tmpStub.transform.SetAsLastSibling();

        //Убираем перетягиваемый элемент
        if (m_DraggingIcon != null)
        {
            m_DraggingIcon.GetComponent<Image>().raycastTarget = true;//в дальнейшем объекты можно тасовать
            m_DraggingIcon = null;
        }
            //Destroy(m_DraggingIcon);
        //Если отпустили не над контентом, не меняем очередь, удаляем объект, который тянули

    }
    */

    #endregion невостребованный код
    //_________________________________
    //Группа "обычных" методов

    public void OnBtnCoin(/*BuildingInProgressInfo*/IInProgressInfo myProgressInfo)
    {
        //Debug.Log(myBuilding.ToString());        
        if (myProgressInfo is BuildingInProgressInfo)
        {
            Messenger<BuildingInProgressInfo>.Broadcast(
                GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING_BUYOUT,
                (BuildingInProgressInfo) myProgressInfo);
        }//Messenger<SBuilding>.Broadcast(GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING, myBuilding);
    }

    //TODO если прогесс здания отличен от 0, выводить диалоговое окно "Вы уверены, что хотите отменить строительство?"
    public void OnBtnCancel(/*BuildingInProgressInfo*/IInProgressInfo myProgressInfo)
    {
        if (myProgressInfo is BuildingInProgressInfo)
        {
            Messenger<BuildingInProgressInfo>.Broadcast(
                GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING_CANCEL,
                (BuildingInProgressInfo)myProgressInfo);
        }
        Destroy(this.gameObject);
    }

    public void OnBtnPause(/*BuildingInProgressInfo*/IInProgressInfo myProgressInfo)
    {
        Debug.Log("Btn Pause clicked");
        _btnPause.gameObject.SetActive(false);
        _btnUnpause.gameObject.SetActive(true);
        if (myProgressInfo is BuildingInProgressInfo)
        {
            Messenger<BuildingInProgressInfo>.Broadcast(
                GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING_PAUSE,
                (BuildingInProgressInfo)myProgressInfo);
        }
    }

    public void OnBtnUnpause(/*BuildingInProgressInfo*/IInProgressInfo myProgressInfo)
    {
        _btnPause.gameObject.SetActive(true);
        _btnUnpause.gameObject.SetActive(false);
        if (myProgressInfo is BuildingInProgressInfo)
        {
            Messenger<BuildingInProgressInfo>.Broadcast(
                GameEvents.CityGUIGameEvent.ON_BUTTON_BUILDING_UNPAUSE, 
                (BuildingInProgressInfo)myProgressInfo);
        }
    }

    //public bool dragOnSurfaces = false;//TODO разобраться, что давало это поле
    //private void SetDraggedPosition(PointerEventData data)
    //{
    //    if (/*dragOnSurfaces &&*/ data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
    //        m_DraggingPlane = data.pointerEnter.transform as RectTransform;

    //    var rt = m_DraggingIcon.GetComponent<RectTransform>();
    //    Vector3 globalMousePos;
    //    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
    //    {
    //        rt.position = globalMousePos;
    //        rt.rotation = m_DraggingPlane.rotation;
    //    }
    //}
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
