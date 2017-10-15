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
        UIKit.UITextView ActionTextView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActionTextView != null) {
                ActionTextView.Dispose ();
                ActionTextView = null;
            }
        }
    }
}