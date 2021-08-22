using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

namespace LaserTower
{
    public class ButtonAction : MonoBehaviour, IButtonAction
    {
        private IObservable<bool> isPressed;
        public IObservable<bool> IsPressed { get { if (isPressed == null) isPressed = GetPressed(); return isPressed; } }
        private IObservable<bool> GetPressed()
        {
            Button button = GetComponent<Button>();
            var createStream = Observable.Create<bool>(CreateMethod);
            IObservable<bool> isDown = button.OnPointerDownAsObservable()
                .SelectMany(_ => button.UpdateAsObservable())
                .TakeUntil(button.OnPointerUpAsObservable())
                .RepeatUntilDestroy(button)
                .Select(_ => { return true; });
            IObservable<bool> isUp = button.OnPointerUpAsObservable().Select(_ => { return false; });
            return Observable.Merge(createStream, isDown, isUp);
        }
        IDisposable CreateMethod(IObserver<bool> observer)
        {
            observer.OnNext(false);
            observer.OnCompleted();
            return null;
        }
    }
}

