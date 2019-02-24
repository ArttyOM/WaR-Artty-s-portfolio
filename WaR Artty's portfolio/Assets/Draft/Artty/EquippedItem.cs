using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace WAR
{

    public class EquippedItem : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// ссылка на GObj, который породил экземпляр
        /// </summary>
        public GameObject BoundEquipmentGObj { get; private set; }

        /// <summary>
        /// может пригодиться
        /// </summary>
        public Equipment BoundEqupment { get; private set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                BoundEquipmentGObj.SetActive(true);
                Destroy(this.gameObject);
                if (this!=null) Destroy(this);
               // Debug.Log("double click");
            }
        }

        public static EquippedItem CreateComponent(GameObject where, GameObject equipmentGObj, Equipment equipment)
        {
            EquippedItem equippedItem = where.AddComponent<EquippedItem>();
            equippedItem.BoundEquipmentGObj = equipmentGObj;

            equippedItem.BoundEqupment = equipment;

            return equippedItem;
        }

    }
}