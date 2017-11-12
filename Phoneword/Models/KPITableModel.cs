using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using System.Linq;

namespace Phoneword.Models
{
    public class KPITableModel : UITableViewSource
    {
        public delegate void NewPageHandler(object sender, EventArgs e);
        public event NewPageHandler NewPageEvent;

        private List<Kpi> tableItems;
        private string cellIdentifier = "TableCell";
        // private Dictionary<string, List<Kpi>> indexedTableItems;
        Dictionary<string, List<Kpi>> indexedTableItems = new Dictionary<string,List<Kpi>>();

        private Kpi selectedKpi;

        public Kpi getSelected() { return selectedKpi; }

        string[] keys;

        public KPITableModel(List<Kpi> items)
        {
            tableItems = items;
            var count = 0;

            List<Kpi> relatedItems = new List<Kpi>();

            List<Kpi> neededItems = new List<Kpi>();

            foreach(var item in tableItems)
            {
                if(count == 0)
                {
                    relatedItems.Add(item);
                    count += 1;
                }
                else
                {
                    neededItems.Add(item);
                }
            }

            //Create empty section for related
            string related = "Related";
            indexedTableItems.Add(related, new List<Kpi>(relatedItems));
            string needed = "Needed";
            //Create empty section for needed
            indexedTableItems.Add(needed, new List<Kpi>(neededItems));

            keys = indexedTableItems.Keys.ToArray();
;
        }


        public override nint NumberOfSections(UITableView tableView)
        {
            return keys.Length;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //return tableItems.Count;
            return indexedTableItems[keys[section]].Count;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return keys[section];
        }

        //Shortcut could ge here butnot needed

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);


            //Attributes for action title string
            var attributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Blue,
                Font = UIFont.FromName("Courier-Bold", 24), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/


                UnderlineStyle = NSUnderlineStyle.Single
            };

            if (tableItems[indexPath.Row] != null)
            {
                if(indexPath.Row == 0)
                {
                    //cell
                }

                if (indexPath.Row % 2 == 1)
                {
                    //urban science color
                    cell.BackgroundColor = UIColor.FromRGB(117, 190, 66);
                }
                else
                {
                    //lighter color
                    cell.BackgroundColor = UIColor.FromRGB(204, 255, 153);
                    // cell.BackgroundColor = UIColor.FromRGB(204, 200, 153); //Random try for card view
                }
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.TextLabel.TextAlignment = UITextAlignment.Center;
                //cell.TextLabel.Font = UIFontAttributes.Bold;

                //Header Stuff
                //cell.ImageView.Frame 

                //border stuff
                /*cell.Layer.BorderColor = UIColor.FromRGB(204, 255, 153).CGColor; 
                cell.Layer.BorderWidth = 5;
                cell.Layer.MasksToBounds = true;*/

                //cell.Layer.CornerRadius = 6;
                cell.Layer.ShadowOffset = new CoreGraphics.CGSize(2, 2);
                cell.Layer.ShadowOpacity = 5f;
                cell.Layer.ShadowColor = UIColor.Black.CGColor;

               // var actionTitle = "Action ";
                //cell.DetailTextLabel= rndm;
                //var action = tableItems[indexPath.Row].actionP;
                Kpi curKpi = tableItems[indexPath.Row];


                var finalString = "KPI: " + curKpi.name + " \n" + "Segment: " + curKpi.segment + " \n" + "Value: " + curKpi.p_val;
                //var strings = "ACTION 1 \n";
                //var inputs = strings + action;
                var prettyString = new NSMutableAttributedString(finalString);

               /* var endRange = 9;

                if (indexPath.Row >= 9)
                {
                    endRange = 10;
                }*/

               // prettyString.SetAttributes(attributes.Dictionary, new NSRange(0, endRange));

                //Can apply other attributes to the rest of the text
                cell.TextLabel.AttributedText = prettyString;

               // Kpi curKpi = tableItems[indexPath.Row];
               // cell.TextLabel.Text = $"{curKpi.name}: {curKpi.segment}, {string.Format("{0:0.0%}", curKpi.p_val)}";
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
            selectedKpi = tableItems[indexPath.Row];
            NewPageEvent(this, new EventArgs());
        }

    }
}