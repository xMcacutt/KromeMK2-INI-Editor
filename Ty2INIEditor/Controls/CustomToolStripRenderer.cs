using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ty2INIEditor.Controls
{
    public class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
            if (item.Selected || item.DropDown.Visible)
            {
                // Set the desired foreground color when the mouse enters the menu item
                e.TextColor = SettingsHandler.Colors.BackgroundDark;
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
            if (item.Selected || item.DropDown.Visible)
            {
                using (SolidBrush brush = new SolidBrush(SettingsHandler.Colors.MainText))
                {
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
}
