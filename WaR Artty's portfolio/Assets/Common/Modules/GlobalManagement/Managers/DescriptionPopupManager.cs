using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Reflection;

public class DescriptionPopupManager : MonoBehaviour {
	
    	private Camera _camera;
	// Use this for initialization
	void Awake () {
        _camera = GameObject.Find("EyeWorld").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
        {

            Debug.Log("Fire2 pressed");
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log(hitObject.name);

                WithDescription target = hitObject.GetComponent<WithDescription>();
                if (target != null)
                {
                    Debug.Log("Target hit with description: " + target.description);
                }
                //else StartCoroutine(SphereIndicator(hit.point));
                //Debug.Log ("Hit" + hit.point);
            }
        }
	}
}
