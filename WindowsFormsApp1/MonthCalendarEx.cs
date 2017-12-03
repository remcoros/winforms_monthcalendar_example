using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MonthCalendarEx : MonthCalendar
    {
        private DateTime? _mouseOverDate;

        public event EventHandler<DragDateEventArgs> DragEnterDate;
        public event EventHandler<DragDateEventArgs> DragLeaveDate;
        public event EventHandler<DragDateEventArgs> DragOverArea;
        public event EventHandler<DragDateEventArgs> DragDropDate;

        public MonthCalendarEx()
        {
            DragOver += OnDragDragOver;
            DragLeave += OnDragLeave;
            DragDrop += OnDragDrop;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            if (_mouseOverDate.HasValue)
            {
                RemoveBoldedDate(_mouseOverDate.Value);
                UpdateBoldedDates();
                _mouseOverDate = null;
            }

            var p = PointToClient(new Point(e.X, e.Y));
            var info = HitTest(p);

            var dragArgs = new DragDateEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect, info.HitArea, info.Time);
            OnDragDropDate(dragArgs);
            e.Effect = dragArgs.Effect;
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            if (_mouseOverDate.HasValue)
            {
                RemoveBoldedDate(_mouseOverDate.Value);
                UpdateBoldedDates();
                _mouseOverDate = null;
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED to fix flicker of controls
        //        return cp;
        //    }
        //}

        private void OnDragDragOver(object sender, DragEventArgs e)
        {
            var p = PointToClient(new Point(e.X, e.Y));
            var info = HitTest(p);

            var oldEffect = e.Effect;
            // Winforms/native monthcalendar doesn't support hittest on Next/PreviousMonthDate :(
            var date = info.HitArea == HitArea.Date
                       || info.HitArea == HitArea.WeekNumbers
                ? info.Time
                : (DateTime?)null;

            var dragArgs = new DragDateEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect, info.HitArea, date);
            OnDragOverArea(dragArgs);
            if (dragArgs.Effect != oldEffect)
            {
                e.Effect = dragArgs.Effect;
            }

            if (info.HitArea == MonthCalendar.HitArea.Date
                // PrevMonthDate / NextMonthDate don't give us a Date :(
                // || info.HitArea == MonthCalendar.HitArea.PrevMonthDate 
                // || info.HitArea == MonthCalendar.HitArea.NextMonthDate
            )
            {
                if (_mouseOverDate.HasValue)
                {
                    if (_mouseOverDate == info.Time)
                    {
                        return;
                    }

                    RemoveBoldedDate(_mouseOverDate.Value);
                }

                _mouseOverDate = info.Time;
                AddBoldedDate(info.Time);
                UpdateBoldedDates();

                var enterdragArgs = new DragDateEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect, info.HitArea, info.Time);
                OnDragEnterDate(enterdragArgs);
                e.Effect = enterdragArgs.Effect;
            }
            else
            {
                if (_mouseOverDate.HasValue)
                {
                    var oldDate = _mouseOverDate.Value;

                    RemoveBoldedDate(_mouseOverDate.Value);
                    UpdateBoldedDates();
                    _mouseOverDate = null;

                    var enterdragArgs = new DragDateEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect, info.HitArea, oldDate);
                    OnDragLeaveDate(enterdragArgs);
                    e.Effect = enterdragArgs.Effect;
                }
            }
        }

        protected virtual void OnDragEnterDate(DragDateEventArgs e)
        {
            DragEnterDate?.Invoke(this, e);
        }

        protected virtual void OnDragOverArea(DragDateEventArgs e)
        {
            DragOverArea?.Invoke(this, e);
        }

        protected virtual void OnDragLeaveDate(DragDateEventArgs e)
        {
            DragLeaveDate?.Invoke(this, e);
        }
        
        protected virtual void OnDragDropDate(DragDateEventArgs e)
        {
            DragDropDate?.Invoke(this, e);
        }
    }
}