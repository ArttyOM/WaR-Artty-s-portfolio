using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace WAR
{
    public class ArmorSlot : MonoBehaviour {

        private EquipmentType _slot;
        public EquipmentType Slot {
            get { return _slot; }
            }

        private void Awake()
        {
            if (Enum.TryParse<EquipmentType>(this.gameObject.name, out _slot))
            { }
            else Debug.Log("Не удалось преобразовать GObject " + this.gameObject.name + " в экземпляр enum-а EquipmentType");
        }
    }
}