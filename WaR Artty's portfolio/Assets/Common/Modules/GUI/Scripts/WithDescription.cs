using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithDescription : MonoBehaviour, IWithDescriprion {

    public string description { get; set; }
    void Awake()
    {
        description = "qq";
    }
}
