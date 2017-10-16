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


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //return tableItems.Length; //Number of rows in length
            //return 1;

            return tableItems.Count;
            
            //return 5;
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

        public TableActionModel(List<KpiAction> items)
        {
            tableItems = items;
        }
    }
}
