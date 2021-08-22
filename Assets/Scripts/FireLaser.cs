using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace LaserTower
{
    public class FireLaser : MonoBehaviour,IFireLaser
    {
        [SerializeField]
        private float maxRayDistance;
        public float MaxRayDistance { get => maxRayDistance; }
        [SerializeField]
        private float maxRayPower;
        public float MaxRayPower { get => maxRayPower; }
        [SerializeField]
        private Inputs inputs;
        void Start()
        {
            GameObject gun = GameObject.Find("Gun");
            GameObject startPoint = GameObject.Find("StartPoint");
            List<Vector3> listOfPoint = new List<Vector3>();
            inputs.FireAction
                .Where(x => x == true)
                .Subscribe(_ => CurrentLaserTrace(gun, startPoint, listOfPoint))
                .AddTo(this);
            inputs.FireAction
                .Where(x => x == false)
                .Subscribe(_ =>
                {
                    if (startPoint.TryGetComponent<LineRenderer>(out LineRenderer line)) line.positionCount = 0;
                    else throw new ArgumentNullException($"There is no LineRenderer in { startPoint.name}");
                })
                .AddTo(this);
        }
        public void CurrentLaserTrace(GameObject gun, GameObject startPoint, List<Vector3> listOfPoint)
        {
            float currentDistance = this.MaxRayDistance;
            float currentRayPower = this.MaxRayPower;
            Ray ray = new Ray(startPoint.transform.position, startPoint.transform.position - gun.transform.position);
            listOfPoint.Clear();
            listOfPoint.Add(startPoint.transform.position);
            while (currentDistance > 0 && currentRayPower > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, currentDistance))
                {
                    Vector3 hitPoint = hit.point;
                    listOfPoint.Add(hitPoint);
                    currentDistance -= Vector3.Distance(ray.origin, hitPoint);
                    if (hit.collider.TryGetComponent<IMirror>(out IMirror mirror))
                    {
                        currentRayPower -= mirror.AbsorptionCoefficient;
                        ray = new Ray(hitPoint, Vector3.Reflect(ray.origin, hit.normal));
                    }
                    else
                    {
                        DrawLaserTrace(startPoint, listOfPoint);
                        currentDistance = 0;
                        throw new ArgumentNullException($"There is no IMirror in { hit.collider.gameObject.name}");
                    }
                }
                else
                {
                    listOfPoint.Add(ray.GetPoint(currentDistance));
                    currentDistance = 0;
                }
            }
            DrawLaserTrace(startPoint, listOfPoint);
        }
        private void DrawLaserTrace(GameObject startPoint, List<Vector3> listOfPoint)
        {
            if (startPoint.TryGetComponent<LineRenderer>(out LineRenderer line))
            {
                line.positionCount = listOfPoint.Count;
                line.SetPositions(listOfPoint.ToArray());
            }
            else throw new ArgumentNullException($"There is no LineRenderer in { startPoint.name}");
        }
    }
}
