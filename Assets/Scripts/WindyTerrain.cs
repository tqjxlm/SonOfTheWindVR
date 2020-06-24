using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyTerrain : MonoBehaviour
{
    public float decayRate;

    private TerrainData terrainData;
    private float initSpeed;
    private float initSize;
    private float initBending;

    // Use this for initialization
    void Start()
    {
        terrainData = this.GetComponent<Terrain>().terrainData;

        initSpeed = terrainData.wavingGrassSpeed;
        initSize = terrainData.wavingGrassAmount;
        initBending = terrainData.wavingGrassStrength;
    }

    // Update is called once per frame
    void Update()
    {
        if (terrainData.wavingGrassSpeed > initSpeed)
        {
            terrainData.wavingGrassSpeed *= decayRate;
        }
        if (terrainData.wavingGrassAmount > initSize)
        {
            terrainData.wavingGrassAmount *= decayRate;
        }
        if (terrainData.wavingGrassStrength > initBending)
        {
            terrainData.wavingGrassStrength *= decayRate;
        }
    }

    public void trigger(float speed, float size, float bending)
    {
        terrainData.wavingGrassSpeed = speed;
        terrainData.wavingGrassAmount = size;
        terrainData.wavingGrassStrength = bending;
    }
}
