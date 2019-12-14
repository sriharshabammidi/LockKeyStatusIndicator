using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lock_Key_Status_Indicator
{
    public partial class LockKeyStatusForm : Form
    {
        private Dictionary<Keys, Label> _LockKeyLablePairs;
        private List<Keys> _LocKeys;
        private readonly Color _LockOffColor = Color.LightGreen, _LockOnColor = Color.LightSalmon;

        public LockKeyStatusForm()
        {
            ConfigureFormPositonandSizing();
            SetupForm();
            this.Controls.AddRange(_LockKeyLablePairs.Values.ToArray());
            InitializeComponent();
            Application.Idle += KeyStatusChangeListner;
        }

        void KeyStatusChangeListner(object sender, EventArgs e)
        {
            _LocKeys.ForEach(lockKey => CheckKeyStatusAndUpdateLabel(lockKey));
        }

        private void CheckKeyStatusAndUpdateLabel(Keys keyName) {
            if (IsKeyLocked(keyName))
                _LockKeyLablePairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.BackColor = _LockOnColor;
            else
                _LockKeyLablePairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.BackColor = _LockOffColor;
        }

        private void ConfigureFormPositonandSizing()
        {
            this.TopMost = true;
            // Define the border style of the form to a dialog box.
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            this.MaximizeBox = false;

            // Set the MinimizeBox to false to remove the minimize box.
            this.MinimizeBox = false;

            // Set the start position of the form to the center of the screen.
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - 155, workingArea.Bottom - 100);
        }

        private void SetupForm() {
            int locationCoOrdinateX = 50, locationCoOrdinateY = 25;
            _LockKeyLablePairs = new Dictionary<Keys, Label>();
            _LocKeys = new List<Keys>
            {
                Keys.CapsLock,
                Keys.NumLock,
                Keys.Scroll
            };
            _LocKeys.ForEach(lockKey => {
                Label label = CreateLable(20,20,string.Empty,new Point(locationCoOrdinateX,locationCoOrdinateY));
                _LockKeyLablePairs.Add(lockKey, label);
                locationCoOrdinateX += 20;
            });

            SetLabelTexts(Keys.CapsLock,"A");
            SetLabelTexts(Keys.NumLock,"1");
            SetLabelTexts(Keys.Scroll, "⭳");
        }

        private void SetLabelTexts(Keys keyName, string lableText) { 
            _LockKeyLablePairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.Text = lableText;
        }

        private Label CreateLable(int height, int width, string text, Point location)
        {
            return new Label
            {
                Height = height,
                Width = width,
                Text = text,
                Location = location,
                BorderStyle = BorderStyle.FixedSingle,
            };
        }
    }
}
