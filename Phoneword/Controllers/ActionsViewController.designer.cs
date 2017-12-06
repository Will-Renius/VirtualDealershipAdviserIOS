// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

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
