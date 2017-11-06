using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace Phoneword.Models
{

    public class TableActionModel : UITableViewSource
    {
        List<KpiAction> tableItems;
        string cellIdentifier = "TableCell";
        ActionsViewController owner;


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Count; //Number of rows in table
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

            //cell.TextLabel.Text = tableItems[indexPath.Row];
            if (tableItems[indexPath.Row] != null){
                cell.TextLabel.Text = tableItems[indexPath.Row].actionP;
            }

            else
            {
                cell.TextLabel.Text = "Not Good";
            }
            return cell;
            //If table scrows out of view, a cell is unseable then cell returned as recycled cell.
            //No need for reloading etc.
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            owner.setSelectedKpi(tableItems[indexPath.Row]); //Harry: Page halts, doesn't let you pick different action rows like you can with kpi
            //Error: System.NullReferenceException: Object reference not set to an instance of an object

            //Dan, Fixed it, the owner value was not added to the TableActionModel before
        }


        public TableActionModel(List<KpiAction> items, ActionsViewController Owner)
        {
            tableItems = items;
            owner = Owner;
        }
    }
}
