using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace WAR.GameEngine
{
    public enum FoodIncrementFactors { baseValue,districts, buildings,bonusTiles }
    public enum FoodDecrementFactors { population, units , events }

    public enum CityMoneyIncrementFactors { baseValue, buildings, districts, bonusTiles, Trade}
    public enum CityMoneyDecrementFactors { enemyRaiding}
    public enum FactionMoneyIncrementFactors { baseValue, events, cities, raidingProfit}
    public enum FactionMoneyDecrementFactors { units, events, cities, enemyRaiding}



    /// <summary>
    /// WARNING! не использовать new ResourceChangeFactors<T>() (я не нашел как в Generic закрыть базовый конструктор)
    /// Пользуйтесь фабричным методом ResourceChangeFactors<T>.Create
    /// универсальный класс, используется для работы с факторами прироста и убыли float-значений сущностей.
    /// Например: общий прирост еды (без учета расходов) = сумма базовой величины еды, притока с районов и бонусных тайлов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceChangeFactors<T>
    {
        private ResourceChangeFactors()//прячем конструктор
        { }
        
        /// <summary>
        /// фабричный метод
        /// </summary>
        public static ResourceChangeFactors<T> CreateInstance()
        {
            var f = new ResourceChangeFactors<T>();

            Type typeOfT = typeof(T);
            if (typeOfT.IsEnum)
            {
                foreach (var t in typeOfT.GetEnumValues())
                {
                    f.Factors.Add((T)t, 0f);
                }
            }
            else
            {
                Debug.Log("На вход ResourceChangeFactors<T> подан не Enum!");
                f = null; //дальше дело за сборщиком мусора
            }
            return f;
        }

        public Dictionary<T, float> Factors { get; } = new Dictionary<T, float>(); //основная коллекция класса - список факторов и их величина

        public float Sum { get; private set; } = 0; //сумма всех факторов

        /// <summary>
        /// метод устанавливает значение выбранного фактора на value
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="value"></param>
        public void Set(T factor, float value)
        {
            Sum -= Factors[factor];
            Factors[factor] = value;
            Sum += value;
        }

        /// <summary>
        /// метод увеличивает (или уменьшает) величину выбранного фактора на value 
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="value"></param>
        public void Change(T factor, float value)
        {
            Sum += value;
            Factors[factor] += value;
        }
    }
}