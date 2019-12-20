using LockKeyStatusIndicator.GlobalKeyHookCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LockKeyStatusIndicator
{
    public partial class LockKeyStatusForm : Form
    {
        private Dictionary<Keys, Label> _LockKeyLabelPairs;
        private List<Keys> _LocKeys;
        private readonly Color _LockOffColor = Color.LightGreen, _LockOnColor = Color.LightSalmon;
        private readonly KeyHookController _keyHookController = new KeyHookController();

        public LockKeyStatusForm()
        {
            ConfigureFormPositonandSizing();
            SetupForm();
            this.Controls.AddRange(_LockKeyLabelPairs.Values.ToArray());
            InitializeComponent();
            _keyHookController.SetupKeyboardHooks();
            Application.Idle += KeyStatusChangeListner;
            Application.ApplicationExit += EndKeyHook; 
        }

        void KeyStatusChangeListner(object sender, EventArgs e)
        {
            _LocKeys.ForEach(lockKey => CheckKeyStatusAndUpdateLabel(lockKey));
        }

        private void CheckKeyStatusAndUpdateLabel(Keys keyName)
        {
            bool isCurrentKeyStatusLocked = false;
            switch (keyName)
            {
                case Keys.NumLock:
                    isCurrentKeyStatusLocked = LockKeyStatus.IsNumLockOn;
                    break;
                case Keys.CapsLock:
                    isCurrentKeyStatusLocked = LockKeyStatus.IsCapsLockOn;
                    break;
                case Keys.Scroll:
                    isCurrentKeyStatusLocked = LockKeyStatus.IsScroolLockOn;
                    break;
            }
            if (!isCurrentKeyStatusLocked)
            {
                _LockKeyLabelPairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.BackColor = _LockOnColor;
            }
            else
            {
                _LockKeyLabelPairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.BackColor = _LockOffColor;
            }
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

            // Set the start position of the form to the bottom right of the screen.
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - 155, workingArea.Bottom - 100);
        }

        private void SetupForm()
        {
            int locationCoOrdinateX = 50, locationCoOrdinateY = 25;
            _LockKeyLabelPairs = new Dictionary<Keys, Label>();
            _LocKeys = new List<Keys>
            {
                Keys.CapsLock,
                Keys.NumLock,
                Keys.Scroll
            };
            _LocKeys.ForEach(lockKey =>
            {
                Label label = CreateLabel(20, 20, string.Empty, new Point(locationCoOrdinateX, locationCoOrdinateY));
                _LockKeyLabelPairs.Add(lockKey, label);
                locationCoOrdinateX += 20;
            });

            SetLabelTexts(Keys.CapsLock, "A");
            SetLabelTexts(Keys.NumLock, "1");
            SetLabelTexts(Keys.Scroll, "⭳");
        }

        private void SetLabelTexts(Keys keyName, string labelText)
        {
            _LockKeyLabelPairs.Where(keyLabelPari => keyLabelPari.Key == keyName).First().Value.Text = labelText;
        }

        private static Label CreateLabel(int height, int width, string text, Point location)
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

        private void EndKeyHook(object sender, EventArgs e)
        {
            _keyHookController.Dispose();
        }
    }
}
