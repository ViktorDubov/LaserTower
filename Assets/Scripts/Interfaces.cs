using System.Collections.Generic;
using UnityEngine;
using System;

namespace LaserTower
{
    interface IButtonAction
    {
        IObservable<bool> IsPressed { get; }
    }
    interface IMirror
    {
        float AbsorptionCoefficient { get; }
    }
    interface IFireLaser
    {
        float MaxRayDistance { get; }
        float MaxRayPower { get; }
        public void CurrentLaserTrace(GameObject gun, GameObject startPoint, List<Vector3> listOfPoint);
    }
}
