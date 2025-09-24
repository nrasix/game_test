using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent _navMeshAgent;
    }
}