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
    [Register ("KPIViewController")]
    partial class KPIViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton KPIActionsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton KPITestButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView KPITextView { get; set; }

        [Action ("KPIActionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void KPIActionsButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("KPITestButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void KPITestButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (KPIActionsButton != null) {
                KPIActionsButton.Dispose ();
                KPIActionsButton = null;
            }

            if (KPITestButton != null) {
                KPITestButton.Dispose ();
                KPITestButton = null;
            }

            if (KPITextView != null) {
                KPITextView.Dispose ();
                KPITextView = null;
            }
        }
    }
}