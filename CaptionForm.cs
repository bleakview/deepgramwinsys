using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deepgramwinsys
{
    public partial class CaptionForm : Form
    {
        private bool mouseDown;
        private Point lastPos;
        /// <summary>
        /// Init caption form
        /// </summary>
        public CaptionForm()
        {
            InitializeComponent();
        }
        // Form move code https://stackoverflow.com/a/30245
        private void CaptionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
            }
        }

        private void CaptionForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastPos = MousePosition;
        }

        private void CaptionForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
