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
    [Register ("ActionsViewController")]
    partial class ActionsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EmailButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TakeActionButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EmailButton != null) {
                EmailButton.Dispose ();
                EmailButton = null;
            }

            if (TakeActionButton != null) {
                TakeActionButton.Dispose ();
                TakeActionButton = null;
            }
        }
    }
}