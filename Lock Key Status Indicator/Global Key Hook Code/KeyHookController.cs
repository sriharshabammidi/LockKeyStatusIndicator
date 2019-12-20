using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockKeyStatusIndicator.GlobalKeyHookCode
{
    internal class KeyHookController : IDisposable
    {
        private GlobalKeyboardHook _globalKeyboardHook;
        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
           LockKeyStatus.IsCapsLockOn =  Control.IsKeyLocked(Keys.CapsLock);
           LockKeyStatus.IsNumLockOn = Control.IsKeyLocked(Keys.NumLock);
           LockKeyStatus.IsScroolLockOn = Control.IsKeyLocked(Keys.Scroll);
        }

        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
        }
    }
}
