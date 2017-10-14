using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a hex, including position, size, neighbours, etc. 
public class Hex {

    private static readonly float RADIUS = 1f;

    private static readonly float HEIGHT = RADIUS * 2;
    private static readonly float WIDTH = (Mathf.Sqrt(3) / 2) * HEIGHT;
    private static readonly float HORIZONTAL_SPACING = WIDTH;
    private static readonly float VERTICAL_SPACING = HEIGHT * 0.75f;

    bool allowWrappingEastWest = true;
    bool allowWrappingNorthSouth = false;

    // Q + R + S = 0
    // S = -(Q + R)
    public Hex(int q, int r)
    {
        Q = q;
        R = r;
        S = -(q + r);

        Elevation = -1;
    }

    /// <summary>
    /// Column
    /// </summary>
    public int Q { get; private set; }
    /// <summary>
    /// Row
    /// </summary>
    public int R { get; private set; }
    public int S { get; private set; }

    public GameObject GameObject { get; set; }

    public float Elevation { get; set; }
    public float Moisture { get; set; }

    public static float Distance(Hex a, Hex b)
    {
        return Mathf.Max(
            Mathf.Abs(a.Q - b.Q),
            Mathf.Abs(a.R - b.R),
            Mathf.Abs(a.S - b.S));
    }

    /// <summary>
    /// Returns the worldspace position of this hex
    /// </summary>
    /// <returns></returns>
    public Vector3 Position()
    {
        return new Vector3(HORIZONTAL_SPACING * (Q + R / 2f), 0, VERTICAL_SPACING * R);
    }

    public Vector3 PositionFromCamera(Vector3 cameraPosition, int colCount, int rowCount)
    {
        Vector3 position = Position();

        if (allowWrappingEastWest)
        {
            position.x -= DoEastWestWrapping(cameraPosition, colCount);            
        } 

        if (allowWrappingNorthSouth)
        {
            position.z -= DoNorthSouthWrapping(cameraPosition, rowCount);
        }

        return position;
    }

    private float DoNorthSouthWrapping(Vector3 cameraPosition, int rowCount)
    {
        Vector3 position = Position();

        float mapHeight = rowCount * VERTICAL_SPACING;
        float howManyHeightsFromCamera = (position.z - cameraPosition.z) / mapHeight;

        // We are too far from the camera. We have to move our hex to the left or right edge of the camera
        if (howManyHeightsFromCamera > 0)
        {
            howManyHeightsFromCamera += 0.5f;
        }
        else
        {
            howManyHeightsFromCamera -= 0.5f;
        }

        int howManyHeightsToFix = (int)howManyHeightsFromCamera;

        return howManyHeightsToFix * mapHeight;
    }

    public float DoEastWestWrapping(Vector3 cameraPosition, int colCount)
    {
        Vector3 position = Position();

        float mapWidth = colCount * HORIZONTAL_SPACING;
        float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;

        // The goal is to keep howManyWidthsFromCamera from -0.5 to 0.5
        if (Mathf.Abs(howManyWidthsFromCamera) <= 0.5f)
        {
            // We're good
            return 0;
        }

        // We are too far from the camera. We have to move our hex to the left or right edge of the camera
        // If we are at 0.6, move to -0.4
        // if we are att 2.2, move to 0.2

        if (howManyWidthsFromCamera > 0)
        {
            howManyWidthsFromCamera += 0.5f;
        }
        else
        {
            howManyWidthsFromCamera -= 0.5f;
        }

        int howManyWidthToFix = (int)howManyWidthsFromCamera;

        return howManyWidthToFix * mapWidth;
    }
}
