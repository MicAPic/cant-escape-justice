using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardCourtroom : UISwipeableCard<DefendantRecord>
    {
        [SerializeField] 
        private Image body;
        [SerializeField] 
        private Image eyes;
        [SerializeField] 
        private Image mouth;
        [SerializeField] 
        private Image hair;
        
        [SerializeField]
        private CanvasGroup imgLike;

        [SerializeField]
        private CanvasGroup imgNope;

        public override void UpdateContent(DefendantRecord data)
        {
            // bg.color = data.color;
            body.sprite = data.potato;
            eyes.sprite = data.eyes;
            mouth.sprite = data.mouth;
            hair.sprite = data.hair;
            
            imgLike.alpha = 0;
            imgNope.alpha = 0;
        }

        protected override void SwipingRight(float rate)
        {
            imgLike.alpha = rate;
            imgNope.alpha = 0;
        }

        protected override void SwipingLeft(float rate)
        {
            imgNope.alpha = rate;
            imgLike.alpha = 0;
        }
    }    
}

