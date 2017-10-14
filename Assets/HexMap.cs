using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

    public GameObject HexPrefab;

    public Mesh MeshOcean;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;
    
    public Material MaterialOcean;
    public Material MaterialGrass;
    public Material MaterialPlains;
    public Material MaterialMountain;    

    public int NumberOfRows = 30;
    public int NumberOfColumns = 60;

    public bool AllowEastWestWrapping = true;
    public bool AllowNorthSouthWrapping = false;

    private Hex[,] hexes;
    private List<Hex> hexList = new List<Hex>();

    // Use this for initialization
    void Start() {
        hexes = new Hex[NumberOfColumns, NumberOfRows];
        GenerateMap();
    }

    public Hex GetHexAt(int col, int row)
    {
        if (AllowEastWestWrapping)
        {
            col = col % NumberOfColumns;
        }
        else if (col < 0 || col >= NumberOfColumns)
        {
            return null;
        }

        if (AllowNorthSouthWrapping)
        {
            row = row % NumberOfRows;
        }
        else if (row < 0 || row >= NumberOfRows)
        {
            return null;
        }

        return hexes[col, row];
    }

    public List<Hex> GetAllHexes()
    {
        return hexList;
    }

    // Generate ocean map
    public virtual void GenerateMap()
    {
        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                Hex h = new Hex(column, row);
                hexes[column, row] = h;
                hexList.Add(h);

                // instantiate a hex
                GameObject hexGO = (GameObject)Instantiate(HexPrefab, h.PositionFromCamera(Camera.main.transform.position, NumberOfColumns, NumberOfRows), Quaternion.identity, this.transform);

                hexGO.name = string.Format("Hex {0},{1}", column, row);
                hexGO.GetComponent<HexBehaviour>().Hex = h;
                hexGO.GetComponent<HexBehaviour>().HexMap = this;
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                h.GameObject = hexGO;
            }
        }
    }

    protected void UpdateHexVisuals()
    {
        foreach (Hex h in GetAllHexes())
        {
            MeshRenderer mr = h.GameObject.GetComponentInChildren<MeshRenderer>();

            if (h.Elevation >= 0)
            {
                mr.material = MaterialGrass;
            } else
            {
                mr.material = MaterialOcean;
            }

            

            MeshFilter mf = h.GameObject.GetComponentInChildren<MeshFilter>();
            mf.mesh = MeshOcean;
        }
    }

    public List<Hex> GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> result = new List<Hex>();

        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = Mathf.Max(-range + 1, -dx-range); dy < Mathf.Min(range, -dx + range - 1); dy++)
            {
                int hexCol = MassageHexIndex(centerHex.Q + dx, AllowEastWestWrapping, NumberOfColumns);
                int hexRow = MassageHexIndex(centerHex.R + dy, AllowNorthSouthWrapping, NumberOfRows);

                if (!result.Contains(hexes[hexCol, hexRow])) {
                    result.Add(hexes[hexCol, hexRow]);
                }
            }
        }

        return result;
    }

    public int MassageHexIndex(int desiredIndex, bool wrappingEnabled, int maxIndex)
    {
        if (wrappingEnabled)
        {
            if (desiredIndex < 0)
            {
                desiredIndex = maxIndex + desiredIndex;
            }
            else if (desiredIndex >= maxIndex)
            {
                desiredIndex = desiredIndex % maxIndex;
            }
        }
        else
        {
            desiredIndex = Mathf.Max(0, desiredIndex);
            desiredIndex = Mathf.Min(desiredIndex, maxIndex - 1);
        }

        return desiredIndex;
    }
}
