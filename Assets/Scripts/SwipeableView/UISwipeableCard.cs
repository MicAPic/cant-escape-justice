using System;
using System.Collections;
using UnityEngine;
using System.Text;

namespace SwipeableView
{
    public class UISwipeableCard<TData, TContext> : MonoBehaviour, ISwipeable where TContext : class
    {
        public bool isGuilty;
        public string charge;
        public string schedule;
        public string timeOfCrime;

        [SerializeField] SwipeableViewData _viewData = default;

        /// <summary>
        /// Index of Card Data.
        /// </summary>
        public int DataIndex { get; set; }

        /// <summary>
        /// Callbacks
        /// </summary>
        public event Action<UISwipeableCard<TData, TContext>> ActionSwipedRight;
        public event Action<UISwipeableCard<TData, TContext>> ActionSwipedLeft;
        public event Action<UISwipeableCard<TData, TContext>, float> ActionSwipingRight;
        public event Action<UISwipeableCard<TData, TContext>, float> ActionSwipingLeft;

        protected TContext Context { get; private set; }

        public RectTransform cachedRect;
        int _screenSize;

        private const float _epsion = 1.192093E-07f;

        void OnEnable()
        {
            cachedRect = transform as RectTransform;
            _screenSize = Screen.height > Screen.width ? Screen.width : Screen.height;
        }

        void Update()
        {
            var rectPosX = cachedRect.localPosition.x;
            if (Math.Abs(rectPosX) < _epsion)
            {
                SwipingRight(0);
                SwipingLeft(0);
                return;
            }

            var t = GetCurrentPosition(rectPosX);
            var maxAngle = rectPosX < 0 ? _viewData.MaxInclinationAngle : -_viewData.MaxInclinationAngle;
            UpdateRotation(Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, maxAngle), t));

            if (rectPosX > 0)
            {
                SwipingRight(t);
                ActionSwipingRight?.Invoke(this, t);
            }
            else if (rectPosX < 0)
            {
                SwipingLeft(t);
                ActionSwipingLeft?.Invoke(this, t);
            }
        }


        /// <summary>
        /// Updates the Content.
        /// </summary>
        /// <param name="data"></param>
        public virtual void UpdateContent(TData data)
        { }

        /// <summary>
        /// Set the Context.
        /// </summary>
        /// <param name="context"></param>
        public virtual void SetContext(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Set the visible.
        /// </summary>
        /// <param name="visible"></param>
        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position"></param>
        public virtual void UpdatePosition(Vector3 position)
        {
            cachedRect.localPosition = position;
        }

        /// <summary>
        /// Updates the rotaion.
        /// </summary>
        /// <param name="rotation"></param>
        public virtual void UpdateRotation(Vector3 rotation)
        {
            cachedRect.localEulerAngles = rotation;
        }

        /// <summary>
        /// Update the scale.
        /// </summary>
        /// <param name="scale"></param>
        public virtual void UpdateScale(float scale)
        {
            cachedRect.localScale = scale * Vector3.one;
        }

        /// <summary>
        /// Right swiping.
        /// </summary>
        /// <param name="rate"></param>
        protected virtual void SwipingRight(float rate)
        { }

        /// <summary>
        /// Left swiping.
        /// </summary>
        /// <param name="rate"></param>
        protected virtual void SwipingLeft(float rate)
        { }

#region ISwipeable
        public void Swipe(Vector2 position)
        {
            UpdatePosition(cachedRect.localPosition + new Vector3(position.x, position.y, 0));
        }

        public void EndSwipe()
        {
            var dialogueManager = FindObjectOfType<DialogueManager>();
            
            // over required distance -> Auto swipe
            if (IsSwipedRight(cachedRect.localPosition))
            {
                AutoSwipeRight(cachedRect.localPosition);
                if (dialogueManager)
                {
                    dialogueManager.SelectChoice(1);
                }
                Debug.Log($"You swiped right and the defendant is guilty: {isGuilty}");
            }
            else if (IsSwipedLeft(cachedRect.localPosition))
            {
                Debug.Log($"You swiped left and the defendant is guilty: {isGuilty}");
                if (dialogueManager)
                {
                    dialogueManager.SelectChoice(0);
                }
                AutoSwipeLeft(cachedRect.localPosition);
            }
            // Not been reached required distance -> Return to default position
            else
            {
                StartCoroutine(MoveCoroutine(cachedRect.localPosition, Vector3.zero));
                return;
            }

            if (!dialogueManager)
            {
                // update the next defendant's record 
                GameManager.Instance.caseCounters[1].text = $"#{DataIndex + 2}";
                
                var nextCase = FindObjectOfType<UISwipeableViewCourtroom>()._data[DataIndex + 1];
                StringBuilder description = new StringBuilder(100);
                description.Append("Defendant is accused of ");
                description.Append($"{nextCase.charge} at <color=#000>{nextCase.timeOfCrime}</color> using <color=#000>carrot.png</color>.");
                GameManager.Instance.caseDescriptions[1].text = description.ToString();
                GameManager.Instance.caseSchedules[1].text = nextCase.schedule;
                
                Debug.Log($"{nextCase.charge} {nextCase.timeOfCrime} {nextCase.isGuilty}");
            }

            // show it
            GameManager.Instance.SwitchCases();
        }

        public void AutoSwipeRight(Vector3 from)
        {
            var vec = from != Vector3.zero ? (from - Vector3.zero).normalized : Vector3.right;
            var to = vec * _screenSize;
            StartCoroutine(MoveCoroutine(from, to, () => ActionSwipedRight?.Invoke(this)));
        }

        public void AutoSwipeLeft(Vector3 from)
        {
            var vec = from != Vector3.zero ? (from - Vector3.zero).normalized : Vector3.left;
            var to = vec * _screenSize;
            StartCoroutine(MoveCoroutine(from, to, () => ActionSwipedLeft?.Invoke(this)));
        }
#endregion

        bool IsSwipedRight(Vector3 position)
        {
            return position.x > 0 && position.x > GetRequiredDistance(position.x);
        }

        bool IsSwipedLeft(Vector3 position)
        {
            return position.x < 0 && position.x < GetRequiredDistance(position.x);
        }

        float GetRequiredDistance(float positionX)
        {
            return positionX > 0 ? cachedRect.rect.size.x / 2 : -(cachedRect.rect.size.x / 2);
        }

        float GetCurrentPosition(float positionX)
        {
            return positionX / GetRequiredDistance(positionX);
        }

        IEnumerator MoveCoroutine(Vector3 from, Vector3 to, Action onComplete = null)
        {
            float endTime = Time.time + _viewData.SwipeDuration;

            while (true)
            {
                float diff = endTime - Time.time;
                if (diff <= 0)
                {
                    break;
                }

                var rate = 1 - Mathf.Clamp01(diff / _viewData.SwipeDuration);
                var t = _viewData.CardAnimationCurve.Evaluate(rate);
                cachedRect.localPosition = Vector3.Lerp(from, to, t);
                yield return null;
            }

            cachedRect.localPosition = to;
            onComplete?.Invoke();
        }
    }

    public class UISwipeableCard<TData> : UISwipeableCard<TData, SwipeableViewNullContext>
    { }
}