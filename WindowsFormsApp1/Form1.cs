using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            monthCalendar1.DragEnterDate += MonthCalendar1OnDragEnterDate;
            monthCalendar1.DragLeaveDate += MonthCalendar1OnDragLeaveDate;
            monthCalendar1.DragOverArea += MonthCalendar1OnDragOverArea;
        }

        private void MonthCalendar1OnDragOverArea(object sender, DragDateEventArgs e)
        {
            if (e.Time != null && e.Data.GetDataPresent(DataFormats.Text))
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MonthCalendar1OnDragLeaveDate(object sender, DragDateEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void MonthCalendar1OnDragEnterDate(object sender, DragDateEventArgs e)
        {
            Debug.WriteLine($"DragEnterDate {e.Time}");
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED to fix flicker of controls
                return cp;
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            button1.DoDragDrop(button1.Text, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void monthCalendar1_DragEnter(object sender, DragEventArgs e)
        {


        }

        private void monthCalendar1_DragDrop(object sender, DragEventArgs e)
        {

        }
    }
}
