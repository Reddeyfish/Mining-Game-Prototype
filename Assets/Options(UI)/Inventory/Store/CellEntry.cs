using UnityEngine;
using System.Collections;

public class CellEntry : MonoBehaviour {
    public bool Empty { get; set; }
    void Awake()
    {
        Empty = true;
    }
}
