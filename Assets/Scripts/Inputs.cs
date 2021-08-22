using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace LaserTower
{
    public class Inputs : MonoBehaviour
    {
        [SerializeField]
        private Button up, down, right, left, fire;

        private IObservable<Vector2> moveAction;
        public IObservable<Vector2> MoveAction { get { if (moveAction == null) moveAction = GetMove(); return moveAction; } }
        
        private IObservable<bool> fireAction;
        public IObservable<bool> FireAction { get { if (fireAction == null) fireAction = GetFire(); return fireAction; } }

        private IObservable<Vector2> GetMove()
        {
            if (up.gameObject.TryGetComponent<IButtonAction>(out IButtonAction upButton) &&
            down.gameObject.TryGetComponent<IButtonAction>(out IButtonAction downButton) &&
            right.gameObject.TryGetComponent<IButtonAction>(out IButtonAction rightButton) &&
            left.gameObject.TryGetComponent<IButtonAction>(out IButtonAction leftButton))
            {
                return Observable.CombineLatest(upButton.IsPressed, downButton.IsPressed, rightButton.IsPressed, leftButton.IsPressed)
                    .Select(x => { return new Vector2((x[0] ? 1 : 0) + (x[1] ? -1 : 0), (x[2] ? 1 : 0) + (x[3] ? -1 : 0)); });
            }
            else throw new ArgumentNullException("There are no IButtonAction in move Buttons");
        }
        private IObservable<bool> GetFire()
        {
            if (fire.gameObject.TryGetComponent<IButtonAction>(out IButtonAction buttonAction)) return buttonAction.IsPressed;
            else throw new ArgumentNullException("There is no IButtonAction in fire Button");
        }
    }
}
