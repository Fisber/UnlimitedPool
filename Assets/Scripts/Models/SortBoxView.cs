

namespace Models
{
    public class SortBoxView
    {
        // should be O(n²)
        private BoxView CurrentLeft;
        private BoxView CurrentRight;
        private int InitialNodeDepthLeft = 5; // 
        private int InitialNodeDepthRight = 5; // 

        private int SortDepth = 11; // 

        public SortBoxView(BoxView boxView)
        {
            Sort(boxView);
        }
        private void Sort(BoxView boxView)
        {
            CurrentLeft = boxView;
            CurrentRight = boxView;
           
            // logic to check if on the edge of the line 

            while (CurrentLeft.PreviosBoxView != null && InitialNodeDepthLeft > 0)
            {
                InitialNodeDepthLeft--;
                CurrentLeft = CurrentLeft.PreviosBoxView;
            }

            while (CurrentRight.NextBoxView != null && InitialNodeDepthRight > 0)
            {
                InitialNodeDepthRight--;
                CurrentRight = CurrentRight.NextBoxView;
            }

            SortDepth -= InitialNodeDepthLeft + InitialNodeDepthRight; // depth of sort 
            for (BoxView current = CurrentLeft; current != null; current = current.NextBoxView)
            {
                if (SortDepth-- <= 0)
                {
                    break;
                }

                for (BoxView index = current; index != null; index = index.NextBoxView)
                {
                    if (current.Value > index.Value)
                    {
                        var tempBoxView = current.Value;
                        current.UpdateText(index.Value); // set value 
                        index.UpdateText(tempBoxView);

                        current.SetVerticalPosition();
                        current.MoveBack();
                    }
                }
            }
        }

   
    }
}