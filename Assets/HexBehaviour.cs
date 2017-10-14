using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBehaviour : MonoBehaviour {
    public Hex Hex { get; set; }
    public HexMap HexMap { get; set; }

    public void UpdatePosition()
    {
        transform.position = Hex.PositionFromCamera(Camera.main.transform.position, HexMap.NumberOfColumns, HexMap.NumberOfRows);
    }
}
