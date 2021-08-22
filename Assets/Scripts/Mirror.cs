using UnityEngine;

namespace LaserTower
{
    public class Mirror : MonoBehaviour, IMirror
    {
        [SerializeField]
        private float absorptionCoefficient;
        public float AbsorptionCoefficient { get => absorptionCoefficient; }
    }

}
