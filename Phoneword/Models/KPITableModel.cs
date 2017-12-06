using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using System.Linq;
using CoreGraphics;

namespace Phoneword.Models
{
    public class KPITableModel : UITableViewSource
    {
        public delegate void NewPageHandler(object sender, EventArgs e);
        public event NewPageHandler NewPageEvent;

        //private List<Kpi> tableItems;
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

        /*public override string TitleForHeader(UITableView tableView, nint section)
        {
            return keys[section];
        }*/

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {

            UILabel headerLabel = new UILabel();

            string headerText = "";

            if (section == 0)
            {
                headerText = "Here Is What I Found To Be The Most Related To Your Question: "; //using 20 font for this
            }
            else
            {

                headerText = "Look Below To See Your Most Needed Areas Of Improvement: ";

            }


            //headerLabel.Text = keys[section];

            headerLabel.Text = headerText;

            headerLabel.Font = UIFont.FromName("HelveticaNeue-Bold", 20);

            headerLabel.BackgroundColor = UIColor.White;

            headerLabel.TextColor = UIColor.Black;

            /*headerLabel.ShadowOffset = new CoreGraphics.CGSize(10, 10);
            //headerLabel.ShadowOpacity = 2f;
            headerLabel.ShadowColor = UIColor.Black;*/

            headerLabel.LineBreakMode = UILineBreakMode.WordWrap;
            headerLabel.TextAlignment = UITextAlignment.Center;
            headerLabel.Lines = 0;

            return headerLabel;

        }

        //Shortcut could ge here butnot needed
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            //cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            //Sets background color to blue upon selection
            /*UIView MyView = new UIView();
            MyView.BackgroundColor = UIColor.FromRGB(61, 131, 244);
            cell.SelectedBackgroundView = MyView;*/

           /* var myView = new UIView(new CGRect(0, 0, 5, cell.Bounds.Height *5));
            myView.BackgroundColor = UIColor.Black;
            cell.AddSubview(myView);*/


            //Attributes for action title string
            var vehicleTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(179,149,86), //Bronze Text Color
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                UnderlineColor = UIColor.FromRGB(179,149,86),
               UnderlineStyle = NSUnderlineStyle.Single
            };

            var percentileTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                // UnderlineColor = UIColor.Green,
                // UnderlineStyle = NSUnderlineStyle.Single
            };

            var performanceTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                // UnderlineColor = UIColor.Green,
                // UnderlineStyle = NSUnderlineStyle.Single
            };

            if (indexedTableItems[keys[indexPath.Section]][indexPath.Row] != null)
            {
                /*if(indexPath.Section == 0)
                {
                    cell.BackgroundColor = UIColor.FromRGB(112,200,47); // Urban Science Color
                }

                if (indexPath.Section == 1)
                {
                    if (indexPath.Row % 2 == 1)
                    {
                        
                        //cell.BackgroundColor = UIColor.FromRGB(176, 225, 141); //Lighter Urban Science Color
                        cell.BackgroundColor = UIColor.FromRGB(219, 241, 203); //Lightest Urban Science
                    }
                    else
                    {
                        
                        cell.BackgroundColor = UIColor.FromRGB(220, 220, 220); //darker gray
                    }
                }


                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.TextLabel.TextAlignment = UITextAlignment.Center;

                cell.Layer.ShadowOffset = new CoreGraphics.CGSize(3, 3);
                cell.Layer.ShadowOpacity = 2f;
                cell.Layer.ShadowColor = UIColor.Black.CGColor;*/


                //Making view for card view appearance

                // 350 is the width                 // UILabel myLabel = new UILabel(new CGRect(minX + 12, minY + 10, 350, 120));                 UILabel myLabel = new UILabel(new CGRect(12,12, 350, 120));                   myLabel.BackgroundColor = UIColor.White;                 //myLabel.Sha                  //Make view for Selection of cell                  UIView MyView = new UIView();                 MyView.BackgroundColor = UIColor.White;//UIColor.FromWhiteAlpha(1.0f, 1.0f);                 myLabel.AddSubview(MyView);                 cell.SelectedBackgroundView = MyView;                  //Selection style                 //cell.SelectionStyle = UITableViewCellSelectionStyle.None;                   myLabel.Layer.BorderWidth = 0.8f;

                //Bronze border color
                myLabel.Layer.BorderColor = UIColor.FromRGB(179,149,86).CGColor;                 myLabel.Layer.CornerRadius = 4.0f;                  myLabel.Layer.MasksToBounds = false;                 //myLabel.Layer.MasksToBounds = true;                  //var shadowPath = UIBezierPath.FromRoundedRect(new CGRect(0.0f, 0.0f, 200.0f, 100.0f), 50.0f);                  //Color for shadow                 myLabel.Layer.ShadowColor = UIColor.Black.CGColor;                  //Set offset for shadow                 myLabel.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0); //Width and height                  //myLabel.MasksToBound                  myLabel.Layer.ShadowOpacity = 0.1f;                 //myLabel.ShadowOffset = new CoreGraphics.CGSize(10, 10);                 // myLabel.ShadowColor = null;                 //myLabel.Layer.ShadowPath = shadowPath.CGPath;                  //Sets the shadow upon selection, sets the inside of cell to shadowcolor                 UIBezierPath shadowPath = UIBezierPath.FromRoundedRect(myLabel.Bounds, 4.0f); //Bounds, cornerRadius                 myLabel.Layer.ShadowPath = shadowPath.CGPath;                  myLabel.Lines = 0;                 myLabel.LineBreakMode = UILineBreakMode.WordWrap;                 myLabel.TextAlignment = UITextAlignment.Center;                 // myLabel.Text = tableItems[0].actionP;                  //myView.Layer.CornerRadius = 4;                 //cell.ContentView.AddSubview(myView);                  //myLabel.Frame = CoreGraphics.CGRectEdge(myView.Bounds, 8, 8);                  cell.ContentView.AddSubview(myLabel);                   //Can apply other attributes to the rest of the text                 //myLabel.AttributedText = prettyString;


                //Code ends here

                Kpi curKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row];// tableItems[indexPath.Row];

                var vehicleString = curKpi.brand + " " + curKpi.model + " " + curKpi.name + "\n" + "\n";
                var percentileString = string.Format("Percentile: {0:0.0%} ",curKpi.p_val) + "\n" + "\n";

                var performanceString = "Nothing"; //Good, Bad, or Fair
                if (curKpi.p_val >= .50) //under .2 is red
                {
                    performanceString = "Good \n " ;
                    //valueTextattributes.ForegroundColor = UIColor.FromRGB(34, 98, 6); //Dark Green
                                                                                      // prettyString.AddAttribute(NSString ForegrounColor = UIColor.Red,)
                }
                else if ((curKpi.p_val >= .30) && curKpi.p_val < .50)
                {
                    performanceString = "Fair \n ";
                    //valueTextattributes.ForegroundColor = UIColor.Yellow;//UIColor.FromRGB(196,165,9); //Orange
                }
                else if (curKpi.p_val < .20)
                {
                    performanceString = "Bad \n ";
                    //valueTextattributes.ForegroundColor = UIColor.FromRGB(183, 17, 17); //Dark Red
                }

                //var kpiString = "KPI: " + curKpi.name + " \n";
                //var segmentString = "Segment: " + curKpi.segment + " \n";
                //var valueString = string.Format("Value: {0:0.0%} ", curKpi.p_val);

                var prettyString1 = new NSMutableAttributedString(vehicleString);
                var prettyString2 = new NSMutableAttributedString(percentileString);
                //var brandString = new NSMutableAttributedString("Brand: " + curKpi.brand + " \n");
                //var modelString = new NSMutableAttributedString("Model: " + curKpi.model + " \n");
                var prettyString3 = new NSMutableAttributedString(performanceString);

                //Should also try making a blank UIView and try setting background color
                if (curKpi.p_val >= .50) //under .2 is red
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(34,98,6); //Dark Green
                   // prettyString.AddAttribute(NSString ForegrounColor = UIColor.Red,)
                }else if((curKpi.p_val >= .30) && curKpi.p_val < .50)
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(239,208,52); //Darker Yellow
                }else if (curKpi.p_val < .20)
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(183,17,17); //Dark Red
                }

                //Set attribute to the text
                // prettyString.SetAttributes(KpiTextattributes.Dictionary, new NSRange(0, 4));

                var length1 = vehicleString.Length;
                var length2 = percentileString.Length;
                var length3 = performanceString.Length;

                prettyString1.SetAttributes(vehicleTextattributes.Dictionary, new NSRange(0, length1)); //try (lengt)
                prettyString2.SetAttributes(percentileTextattributes.Dictionary, new NSRange(0, length2)); //try (length1+1, length2?)
                //brandString.SetAttributes(segmentTextattributes.Dictionary, new NSRange(0, 6));
                //modelString.SetAttributes(segmentTextattributes.Dictionary, new NSRange(0, 6));
                prettyString3.SetAttributes(performanceTextattributes.Dictionary, new NSRange(0, length3));


                var prettyString = new NSMutableAttributedString() ;
                prettyString.Append(prettyString1);
                prettyString.Append(prettyString2);
                //prettyString.Append(brandString);
                //prettyString.Append(modelString);
                prettyString.Append(prettyString3);

                //Can apply other attributes to the rest of the text
                //cell.TextLabel.AttributedText = prettyString;
                myLabel.AttributedText = prettyString;



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

        //Returns the height of the tablve view cell
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            //return base.GetHeightForRow(tableView, indexPath);
            return 146;
        }

        /*[Foundation.Export("prepareForReuse")]
        public virtual void PrepareForReuse(){
            
        }*/

       /* public override void PrepareForReuse(){
            
        }*/

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            selectedKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row]; //tableItems[indexPath.Row];
            NewPageEvent(this, new EventArgs());
        }
    }
}