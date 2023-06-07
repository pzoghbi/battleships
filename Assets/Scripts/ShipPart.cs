using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShipPart : ScriptableObject
{
    public Vector2 gridPosition;
    public BattleshipData ship;
    public bool isHit = false;
}