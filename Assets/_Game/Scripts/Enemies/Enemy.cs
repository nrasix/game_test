using Game.Services.Character;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private ITarget _targetObject;

        private void Init(ITarget targetObject)
        {
            _targetObject = targetObject;
        }
    }
}