using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ty2INIEditor
{
    internal class RCTableLayoutPanel : TableLayoutPanel
    {
        public void RemoveRow(int rowIndex)
        {
            if (rowIndex >= RowCount)
            {
                return;
            }

            // delete all controls of row that we want to delete
            for (int i = 0; i < ColumnCount; i++)
            {
                var control = GetControlFromPosition(i, rowIndex);
                Controls.Remove(control);
            }

            // move up row controls that comes after row we want to remove
            for (int i = rowIndex + 1; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var control = GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        SetRow(control, i - 1);
                    }
                }
            }

            var removeStyle = RowCount - 1;

            if (RowStyles.Count > removeStyle)
                RowStyles.RemoveAt(removeStyle);

            RowCount--;
        }
    }
}
