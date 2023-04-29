using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeableView
{
    public class UISwipeableCardCourtroom : UISwipeableCard<DefendantRecord>
    {
        [SerializeField] 
        private Image bg;
        
        [SerializeField]
        private CanvasGroup imgLike;

        [SerializeField]
        private CanvasGroup imgNope;

        public override void UpdateContent(DefendantRecord data)
        {
            isGuilty = data.isGuilty;
            charge = data.charge;
            schedual = data.schedule;
            timeOfCrime = data.timeOfCrime;
            
            bg.color = data.color;
            
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

