using System;
using UnityEngine;

namespace Game.Services.Input
{
    public interface IInputService
    {
        event Action<Vector3> MovePerformed;
    }
}