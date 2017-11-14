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
        Dictionary<string, List<Kpi>> indexedTableItems = new Dictionary<string,List<Kpi>>();

        private Kpi selectedKpi;

        public Kpi getSelected() { return selectedKpi; }

        string[] keys;

        public KPITableModel(List<Kpi> neededList, Kpi relatedKpi)
        {
            List<Kpi> relatedItems = new List<Kpi>();
            relatedItems.Add(relatedKpi);

            List<Kpi> neededItems = new List<Kpi>();
            neededItems = neededList;

            //Create empty section for related
            string related = "Related To Your Question: ";
            indexedTableItems.Add(related, new List<Kpi>(relatedItems));
            string needed = "Most Needed Areas Of Improvement: ";
            //Create empty section for needed
            indexedTableItems.Add(needed, new List<Kpi>(neededItems));

            keys = indexedTableItems.Keys.ToArray();
        }


        public override nint NumberOfSections(UITableView tableView)
        {
            return keys.Length;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
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
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            //Attributes for action title string
            var KpiTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Red,
                Font = UIFont.FromName("Courier-Bold", 24), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

               // UnderlineColor = UIColor.Green,
               // UnderlineStyle = NSUnderlineStyle.Single
            };

            var segmentTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("Courier-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                // UnderlineColor = UIColor.Green,
                // UnderlineStyle = NSUnderlineStyle.Single
            };

            var valueTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("Courier-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                // UnderlineColor = UIColor.Green,
                // UnderlineStyle = NSUnderlineStyle.Single
            };

            if (indexedTableItems[keys[indexPath.Section]][indexPath.Row] != null)
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
                }
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.TextLabel.TextAlignment = UITextAlignment.Center;

                cell.Layer.ShadowOffset = new CoreGraphics.CGSize(3, 3);
                cell.Layer.ShadowOpacity = 2f;
                cell.Layer.ShadowColor = UIColor.Black.CGColor;

                Kpi curKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row];// tableItems[indexPath.Row];

                var kpiString = "KPI: " + curKpi.name + " \n";
                var segmentString = "Segment: " + curKpi.segment + " \n";
                var valueString = string.Format("Value: {0:0.0%} ", curKpi.p_val);

                var prettyString1 = new NSMutableAttributedString(kpiString);
                var prettyString2 = new NSMutableAttributedString(segmentString);
                var brandString = new NSMutableAttributedString("Brand: " + curKpi.brand + " \n");
                var modelString = new NSMutableAttributedString("Model: " + curKpi.model + " \n");
                var prettyString3 = new NSMutableAttributedString(valueString);

                //Should also try making a blank UIView and try setting background color
                if (curKpi.p_val >= .50) //under .2 is red
                {
                    KpiTextattributes.ForegroundColor = UIColor.FromRGB(34,98,6); //Dark Green
                   // prettyString.AddAttribute(NSString ForegrounColor = UIColor.Red,)
                }else if((curKpi.p_val >= .30) && curKpi.p_val < .50)
                {
                    KpiTextattributes.ForegroundColor = UIColor.FromRGB(246,133,21); //Orange
                }else if (curKpi.p_val < .20)
                {
                    KpiTextattributes.ForegroundColor = UIColor.FromRGB(183,17,17); //Dark Red
                }

                //Set attribute to the text
                // prettyString.SetAttributes(KpiTextattributes.Dictionary, new NSRange(0, 4));

                prettyString1.SetAttributes(KpiTextattributes.Dictionary, new NSRange(0, 4));
                prettyString2.SetAttributes(segmentTextattributes.Dictionary, new NSRange(0, 8));
                brandString.SetAttributes(segmentTextattributes.Dictionary, new NSRange(0, 6));
                modelString.SetAttributes(segmentTextattributes.Dictionary, new NSRange(0, 6));
                prettyString3.SetAttributes(valueTextattributes.Dictionary, new NSRange(0, 6));


                var prettyString = new NSMutableAttributedString() ;
                prettyString.Append(prettyString1);
                prettyString.Append(prettyString2);
                prettyString.Append(brandString);
                prettyString.Append(modelString);
                prettyString.Append(prettyString3);

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
            selectedKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row]; //tableItems[indexPath.Row];
            NewPageEvent(this, new EventArgs());
        }
    }
}