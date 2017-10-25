// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Phoneword
{
    [Register ("LoginController")]
    partial class LoginController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordTextfield { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UsernameTextfield { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }

            if (PasswordTextfield != null) {
                PasswordTextfield.Dispose ();
                PasswordTextfield = null;
            }

            if (UsernameTextfield != null) {
                UsernameTextfield.Dispose ();
                UsernameTextfield = null;
            }
        }
    }
}