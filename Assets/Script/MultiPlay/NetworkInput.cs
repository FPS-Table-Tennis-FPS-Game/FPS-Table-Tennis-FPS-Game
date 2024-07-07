using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInput : INetworkInput
{
    public const byte MOUSEBUTTON0 = 0x01;
    public const byte MOUSEBUTTON1 = 0x02;
    public Vector3 aimDirection;
}
