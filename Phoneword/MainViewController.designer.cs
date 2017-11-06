// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Phoneword
{
    [Register ("ViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EnterLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HomeDeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem HomeNavItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HomeSubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Listener { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Querybox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView QuestionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SpeakerButton { get; set; }

        [Action ("HomeDeleteButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void HomeDeleteButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (EnterLabel != null) {
                EnterLabel.Dispose ();
                EnterLabel = null;
            }

            if (HomeDeleteButton != null) {
                HomeDeleteButton.Dispose ();
                HomeDeleteButton = null;
            }

            if (HomeNavItem != null) {
                HomeNavItem.Dispose ();
                HomeNavItem = null;
            }

            if (HomeSubmitButton != null) {
                HomeSubmitButton.Dispose ();
                HomeSubmitButton = null;
            }

            if (Listener != null) {
                Listener.Dispose ();
                Listener = null;
            }

            if (Querybox != null) {
                Querybox.Dispose ();
                Querybox = null;
            }

            if (QuestionLabel != null) {
                QuestionLabel.Dispose ();
                QuestionLabel = null;
            }

            if (SpeakerButton != null) {
                SpeakerButton.Dispose ();
                SpeakerButton = null;
            }
        }
    }
}