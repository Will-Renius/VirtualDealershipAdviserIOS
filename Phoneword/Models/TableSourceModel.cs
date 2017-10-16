using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace Phoneword.Models
{
    public class TableSourceModel : UITableViewSource
    {
        List<Kpi> tableItems;
        string cellIdentifier = "TableCell";
        KPIViewController owner;

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Count;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

            if (tableItems[indexPath.Row] != null){
                Kpi curKpi = tableItems[indexPath.Row];
                cell.TextLabel.Text = $"{curKpi.name}: {curKpi.segment}, {string.Format("{0:0.0%}", curKpi.p_val)}";
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
            owner.setSelectedKpi(tableItems[indexPath.Row]);
        }

        public TableSourceModel(List<Kpi> items, KPIViewController Owner)
        {
            tableItems = items;
            owner = Owner;
        }
    }
}
