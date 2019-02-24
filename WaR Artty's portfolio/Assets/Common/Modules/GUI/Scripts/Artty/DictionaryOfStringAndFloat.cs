using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DictionaryOfStringAndFloat : SerializableDictionary<string, float>
{
    //т.к. я хз, как сделать универсально, запилю костыль для своего частного случая
    new public void  OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count < size)
        {
            for (int i = 0; i < keys.Count - size; i++)
            {
                keys.Add("");
                values.Add(0f);
            }
        }

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));



        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);



    }
}