using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalWebsite.Comparers
{
    public class LineUpStartTimeComparer : IComparer<LineUpItem>
    {
        public int Compare(LineUpItem x, LineUpItem y)
        {
            DateTime startTimeX = LineUpItem.DateAndTimeStringToDateTime(x.Date, x.StartTime);
            DateTime startTimeY = LineUpItem.DateAndTimeStringToDateTime(y.Date, y.StartTime);

            return startTimeX.CompareTo(startTimeY);
        }
    }
}