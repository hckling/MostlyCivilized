using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotionHandler : MonoBehaviour {

    Vector3 oldPosition;
    HexBehaviour[] hexes;

    // Use this for initialization
    void Start () {
        oldPosition = transform.position;		
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Code to click and drag camera, WASD, zoom in and out
        if (CameraMoved())
        {
            oldPosition = transform.position;
            MoveMap();
        }
	}

    private bool CameraMoved()
    {
        return transform.position != oldPosition;
    }

    private void MoveMap()
    {
        if (hexes == null)
        {
            hexes = GameObject.FindObjectsOfType<HexBehaviour>();
        }

        foreach(HexBehaviour hex in hexes)
        {
            hex.UpdatePosition();
        }
    }

    public void PanToHex(Hex hex)
    {

    }
}
