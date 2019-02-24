using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WAR;
/// <summary>
/// шмотка на панели выбора
/// </summary>
public  class Equipment : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // оружие для юнита
  //  public HandEquip hadEquip;
   // public UnitInSquadController unit;

    private GameObject _thisGObj;
    //private EquipmentType _slot;
    //public EquipmentType Slot
    //{
    //    get { return _slot; }
    //}

    /// <summary>
    /// фабричный метод для инициализации кнопки на пенели Weapon в конструкторе Юнита
    ///
    /// </summary>
    /// <param name="where">объект, которому добавляем экземпляр скрипта</param>
    /// <returns></returns>
    public static Equipment CreateComponent(GameObject where, Outfit outfit)
    {
        Equipment equipment = where.AddComponent<Equipment>();
        //playerController._playerColor = playerColor;
        //пока что хрен с id экземпляра
        equipment.gameObject.name = outfit.name;
        equipment.equipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType), outfit.type);


        Image tmp = where.GetComponent<Image>();
        //tmp.sprite = Resources.Load<Sprite>("Sword");
        if (tmp != null)
        {
            //Debug.Log("imagePath = " + outfit.imagePath);
            tmp.sprite = Resources.Load(outfit.imagePath, typeof(Sprite)) as Sprite;
        }
        else Debug.Log("пусто");

        equipment.UserDescription = outfit.userDescription;

        equipment._thisGObj = equipment.gameObject;
        equipment._thisImage = tmp; //TODO упростить, можно сделать чуть более читабельно
        //_countOfPlayerControllers++;
        return equipment;

    }

    /// <summary>
    /// фабричный метод для инициализации кнопки на пенели Weapon в конструкторе Юнита
    ///
    /// </summary>
    /// <param name="where">объект, которому добавляем экземпляр скрипта</param>
    /// <returns></returns>
    public static Equipment CreateComponent(GameObject where, string name, EquipmentType type, Sprite sprite/*, HandEquip handequip = null, UnitInSquadController unitm = null*/)
    {
        Equipment equipment = where.AddComponent<Equipment>();
        //playerController._playerColor = playerColor;
        //пока что хрен с id экземпляра
        equipment.gameObject.name = name;
        equipment.equipmentType = type;

        //if (handequip != null)
        //    equipment.hadEquip = handequip;

        //if (unitm != null)
        //    equipment.unit = unitm;


        Image tmp = where.GetComponent<Image>();
        //tmp.sprite = Resources.Load<Sprite>("Sword");
        if (tmp != null)
        {
            //Debug.Log("imagePath = " + outfit.imagePath);
            tmp.sprite = sprite;
        }
        else Debug.Log("пусто");

        equipment.UserDescription = string.Empty;//outfit.userDescription;
        equipment._thisGObj = equipment.gameObject;
        equipment._thisImage = tmp; //TODO упростить, можно сделать чуть более читабельно
        //_countOfPlayerControllers++;
        return equipment;

    }

    public string UserDescription { get; set; }  
    public bool dragOnSurfaces = true;
    public EquipmentType equipmentType;

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    private Image _thisImage;

    //void Awake()
    //{
    //    //_thisImage = this.GetComponent<Image>();

    //    //Sprite sprite = new Sprite();
    //    //sprite = Resources.Load<Sprite>("Shield");
    //    //Debug.Log(sprite);

    //}
    //private List<Vector2Int> possibilityToMove = new List<Vector2Int>();//возможное перемещение фигуры без учета других фигур
    //private List<GameObject> movementHighliters = new List<GameObject>();//подсветка движений

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    void Start()
    {

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponentInParent<EventSystem>();
    }

    #region Drag&Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        Color thisImageColor = _thisImage.color;
        thisImageColor.a = 0.3f; //TODO убрать хардкод //делаем полупрозрачным
        _thisImage.color = thisImageColor;

        //склонируем объект, чтобы позволить его перенести и в случае чего моментально откатить
        //m_DraggingIcon = Instantiate(this.gameObject,canvas.transform);//new GameObject("icon");

        //m_DraggingIcon.GetComponent<Image>().raycastTarget = false;



        //m_DraggingIcon.transform.SetParent(canvas.transform, false);
        //m_DraggingIcon.transform.SetAsLastSibling();

        m_DraggingIcon = new GameObject("icon");

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        var image = m_DraggingIcon.AddComponent<Image>();

        image.sprite = GetComponent<Image>().sprite;
        image.SetNativeSize();

        //var image = m_DraggingIcon.AddComponent<Image>();
        //image.sprite = GetComponent<Image>().sprite;
        //image.SetNativeSize();

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);

    }

    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        ArmorSlot tmpArmorSlot;
        EquippedItem tmpEquippedItem;
        GameObject tmp;
        Image tmpImage;
        bool isActive = true;
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            tmpEquippedItem = result.gameObject.GetComponent<EquippedItem>();
            if ((tmpEquippedItem!=null)&&(tmpEquippedItem.BoundEqupment.equipmentType == this.equipmentType))
            {
                tmpEquippedItem.BoundEquipmentGObj.SetActive(true);
                Destroy(tmpEquippedItem.gameObject);
                if (tmpEquippedItem != null) Destroy(tmpEquippedItem);
            }



            tmpArmorSlot =result.gameObject.GetComponent<ArmorSlot>();

            //if (tmpArmorSlot!=null)
            //    Debug.Log(String.Format("ArmorSlot is {0}, this equipmentTypeIs {1}", tmpArmorSlot.Slot, this.equipmentType));

            if ((tmpArmorSlot == null)||(tmpArmorSlot.Slot != this.equipmentType))
                continue;

            tmp = Instantiate(new GameObject(), result.gameObject.transform);
            tmpImage = tmp.AddComponent<Image>();
            tmpImage.sprite = _thisImage.sprite;
            tmpImage.SetNativeSize();
            EquippedItem.CreateComponent(tmp, _thisGObj, this);
            isActive = false;
            //if (result.gameObject.tag == "equipmentSlot")
            //{

            //this.transform.SetParent(result.gameObject.transform, false);
            //this.transform.localPosition = Vector3.zero;

            //}
            //Debug.Log("Hit " + result.gameObject.name);
            //Debug.Log(this.equipmentType);

            //if (this.hadEquip != null)
            //    GameObject.FindObjectOfType<ArmoryContentMaanger2>().ChangeAmunition(this.hadEquip);

            //if (this.unit != null)
            //    GameObject.FindObjectOfType<ArmoryContentMaanger2>().RecreateUnit(this.unit);
        }

        Color thisImageColor = _thisImage.color;
        thisImageColor.a = 1f; //возвращаем прозрачность к нормальному состоянию
        _thisImage.color = thisImageColor;

        if (m_DraggingIcon != null)
            Destroy(m_DraggingIcon);

        _thisGObj.SetActive(isActive);
    }
    #endregion Drag&Drop

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

    protected IEnumerator SetSize(RectTransform rectTransform)
    {
        while (rectTransform != null) 
        {
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
            //yield return null;
            //yield return new WaitUntil(() => parentRectTransform.rect.width != 0);
            //TODO: убрать хардкод

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  parentRectTransform.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRectTransform.rect.height);

            yield return null;
            //yield return new WaitForSeconds(GlobalFields.MyLazyBD.viewUpdatePeriod);
        }
    }

}
