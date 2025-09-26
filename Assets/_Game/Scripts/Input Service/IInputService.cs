using System;
using UnityEngine;

namespace Game.Services.Input
{
    public interface IInputService
    {
        event Action<Vector3> OnMove;
        event Action<Vector3> OnTouchScreen;

        void SetGameInput(bool value);
    }
}