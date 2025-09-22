using UnityEngine;

namespace Game.Services.Character
{
    public interface ITarget
    {
        Transform Transform { get; }
    }
}