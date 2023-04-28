using System.Collections.Generic;

namespace SwipeableView
{
    public class UISwipeableViewCourtroom : UISwipeableView<DefendantRecord>
    {
        public void UpdateData(List<DefendantRecord> data)
        {
            base.Initialize(data);
        }

        public void MoveToFrontNextCard()
        {
            
        }
    }
}
