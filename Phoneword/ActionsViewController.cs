using Foundation;
using System;
using UIKit;

using Phoneword.Models;
using System.Collections.Generic;

namespace Phoneword
{
    public partial class ActionsViewController : UIViewController
    {

        public List<KpiAction> actions { get; set; }
        public ActionsViewController (IntPtr handle) : base (handle)
        {

        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UITableView _table;
            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height),
                Source = new TableActionModel(actions)
            };
            View.AddSubview(_table);
        }
    }
}