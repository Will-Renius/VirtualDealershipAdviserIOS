﻿using System;
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
        string p_val_string;

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
                //p_val_string = @string.Format("kpi percentile {0:0.0%}", ViewBag.most_related_kpi.p_val);

                //string.Format("kpi percentile {0:0.0%}", ViewBag.most_related_kpi.p_val);
                //cell.TextLabel.Text = tableItems[indexPath.Row].name + ":" + tableItems[indexPath.Row].p_val.ToString().Substring(0,5);
                cell.TextLabel.Text = "Kpi: " + tableItems[indexPath.Row].name + ", " + string.Format("Percentile: {0:0.0%}", tableItems[indexPath.Row].p_val);
            }

            else
            {
                cell.TextLabel.Text = "Not Good";
            }
            return cell;
            //If table scrows out of view, a cell is unseable then cell returned as recycled cell.
            //No need for reloading etc.
        }

        public TableSourceModel(List<Kpi> items)
        {
            tableItems = items;
        }
    }
}
