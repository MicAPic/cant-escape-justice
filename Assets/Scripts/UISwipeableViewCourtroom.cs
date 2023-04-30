using System.Collections.Generic;

namespace SwipeableView
{
    public class UISwipeableViewCourtroom : UISwipeableView<DefendantRecord>
    {
        public bool alreadyInitialized;
        
        public void UpdateData(List<DefendantRecord> data)
        {
            Initialize(data);
        }
    }
}
