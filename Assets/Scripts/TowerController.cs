using UnityEngine;
using UniRx;
using System;

namespace LaserTower
{
    public class TowerController : MonoBehaviour
    {
        public float speed;
        [SerializeField]
        private Inputs inputs;
        private float angleX;
        private float AngleX 
        { 
            get { return angleX; } 
            set 
            { 
                if (value < 5) angleX = 5; 
                else if (value > 85) angleX = 85; 
                else angleX = value; 
            } 
        }
        private float angleY;
        private float AngleY
        {
            get { return angleY; }
            set
            {
                if (value < -85) angleY = -85;
                else if (value > 85) angleY = 85;
                else angleY = value;
            }
        }
        public void Start()
        {
            GameObject gun = GameObject.Find("Gun");
            AngleY = gun.transform.eulerAngles.y;
            AngleX = gun.transform.eulerAngles.x;
            inputs.MoveAction
                .Subscribe(v => RotateLogic(gun,v))
                .AddTo(this);
        }
        private void RotateLogic(GameObject gun, Vector2 inputVector)
        {
            AngleY += speed * inputVector.y;
            AngleX -= speed * inputVector.x;
            Quaternion target = Quaternion.Euler(AngleX, AngleY, 0);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, target, 1);
        }
    }
}

