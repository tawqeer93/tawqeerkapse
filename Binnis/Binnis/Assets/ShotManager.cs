using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shot
{
    public float upPower;
    public float hitPower;
}

public class ShotManager : MonoBehaviour
{
    public Shot upSpin;
    public Shot ground;
}
