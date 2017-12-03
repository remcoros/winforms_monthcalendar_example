using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DragDateEventArgs : DragEventArgs
    {
        public MonthCalendar.HitArea HitArea { get; }
        public DateTime? Time { get; }

        public DragDateEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect, MonthCalendar.HitArea hitArea, DateTime? time = null)
            : base(data, keyState, x, y, allowedEffect, effect)
        {
            HitArea = hitArea;
            Time = time;
        }
    }
}