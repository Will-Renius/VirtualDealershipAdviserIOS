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

        private string cellIdentifier = "TableCell";

        //Initialze a dictionary to store KPI items based on sections (Related and Needed)
        Dictionary<string, List<Kpi>> indexedTableItems = new Dictionary<string,List<Kpi>>();

        private Kpi selectedKpi;

        public Kpi getSelected() { return selectedKpi; }

        string[] keys;

        //Constructor
        public KPITableModel(List<Kpi> neededList, Kpi relatedKpi)
        {
            List<Kpi> relatedItems = new List<Kpi>(); //Have a list for each needed and related key
            relatedItems.Add(relatedKpi);

            List<Kpi> neededItems = new List<Kpi>();
            neededItems = neededList;

            //Create empty section for related
            string related = "Related To Your Question: "; //Key
            indexedTableItems.Add(related, new List<Kpi>(relatedItems)); //value is the list of items

            //Create empty section for needed
            string needed = "Most Needed Areas Of Improvement: ";
            indexedTableItems.Add(needed, new List<Kpi>(neededItems));

            keys = indexedTableItems.Keys.ToArray(); //Different sections as dictionary key
        }

        //Set the number of sections
        public override nint NumberOfSections(UITableView tableView)
        {
            return keys.Length;
        }

        //Sets the number of rows (table items) in each section
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return indexedTableItems[keys[section]].Count;
        }

        //Custom header for displaying sections (Related and Needed)
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            //Creates header object
            UILabel headerLabel = new UILabel();

            string headerText = "";

            if (section == 0)
            {
                headerText = "Here Is What I Found For Your Question: "; //using 20 font for this
            }
            else
            {

                headerText = "Needed Areas Of Improvement: ";

            }

            //Set text and appearance values of header
            headerLabel.Text = headerText;
            headerLabel.Font = UIFont.FromName("HelveticaNeue-Bold", 18);
            headerLabel.BackgroundColor = UIColor.FromRGB(179, 149, 86);
            headerLabel.TextColor = UIColor.White;

            //Makes sure string doesn't truncate
            headerLabel.LineBreakMode = UILineBreakMode.WordWrap;
            headerLabel.TextAlignment = UITextAlignment.Center;
            headerLabel.Lines = 0;

            return headerLabel;

        }

        //Declare height for the section header
        public override nfloat GetHeightForHeader(UITableView tableView, nint section){

            return 40;
        }

        //GetCell method for the table to update with the appropriate content
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { 
            //Initialize cell and declare as reusable
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }


            //Set attributes for each string that will go inside the content of the Card
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

            };

            var performanceTextattributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

            };

            //Initializes cell content inside the table view
            if (indexedTableItems[keys[indexPath.Section]][indexPath.Row] != null)
            {

                //Creates a label to represent a Card in the table                 UILabel myLabel = new UILabel(new CGRect(12,12, 350, 134)); //(x,y,width,height)                 myLabel.BackgroundColor = UIColor.White;

                //Make the backgroundcolor of the selected table cell
                //to stay the same upon selection                 UIView MyView = new UIView();                 MyView.BackgroundColor = UIColor.White;                 myLabel.AddSubview(MyView);                 cell.SelectedBackgroundView = MyView; 
                //Edit the border of the Card                 myLabel.Layer.BorderWidth = 0.8f;
                myLabel.Layer.BorderColor = UIColor.FromRGB(179,149,86).CGColor;                 myLabel.Layer.CornerRadius = 4.0f; 
                //Makes sure the card appears to be above the table view cell
                //Instead of on the same plane (Creates elevated effect)                 myLabel.Layer.MasksToBounds = false;                  //Color for shadow                 myLabel.Layer.ShadowColor = UIColor.Black.CGColor;                  //Set offset for shadow                 myLabel.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0); //Width and height 
                //Set opacity of the selection shadow                 myLabel.Layer.ShadowOpacity = 0.1f;                  //Sets the shadow upon selection, sets the inside of cell to shadowcolor
                //Makes it so the cell has an inner shadow upon selection                 UIBezierPath shadowPath = UIBezierPath.FromRoundedRect(myLabel.Bounds, 4.0f); //Bounds, cornerRadius                 myLabel.Layer.ShadowPath = shadowPath.CGPath; 
                //Makes sure text doesn't truncate at the end of the line                 myLabel.Lines = 0;                 myLabel.LineBreakMode = UILineBreakMode.WordWrap;                 myLabel.TextAlignment = UITextAlignment.Center;

                //Reset the content view of the cell
                //So content doesn't overlap once the cell is reused
                if(cell.ContentView.Subviews.Length != 0)
                {
                    cell.ContentView.Subviews[0].RemoveFromSuperview();
                }

                //Sets the content of the cell to the Card                 cell.ContentView.AddSubview(myLabel);

                //Get the current KPI object that is selected
                Kpi curKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row];

                //If model and brand are "all" don't display them in the Card's content
                var brand = curKpi.brand;
                var model = curKpi.model;
                var brandSpace = " ";
                var modelSpace = " ";
                if(brand == "all"){
                    brand = "";
                    brandSpace = "";
                }

                if (model == "all"){
                    model = "";
                    modelSpace = "";
                }

                //Initialize each string for the Card's content
                var vehicleString = brand + brandSpace + model + modelSpace + curKpi.name + "\n" + "\n";
                var percentileString = string.Format("Percentile: {0:0.0%} ",curKpi.p_val) + "\n" + "\n";

                //Set string of the performance based on how the KPI is performing
                var performanceString = "Nothing"; //Good, Bad, or Fair
                if (curKpi.p_val >= .50) 
                {
                    performanceString = "Good \n " ;
                }
                else if ((curKpi.p_val >= .20) && curKpi.p_val < .50)
                {
                    performanceString = "Okay \n ";
                }
                else if (curKpi.p_val < .20)
                {
                    performanceString = "Bad \n ";
                }

                //Creates mutable string object
                var prettyString1 = new NSMutableAttributedString(vehicleString);
                var prettyString2 = new NSMutableAttributedString(percentileString);
                var prettyString3 = new NSMutableAttributedString(performanceString);

                //Sets color of the performance string based on value
                if (curKpi.p_val >= .50) 
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(112,200,47); //Urban Science Green
                }else if((curKpi.p_val >= .20) && curKpi.p_val < .50)
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(0,159,194); //Urban Science Blue
                }else if (curKpi.p_val < .20)
                {
                    performanceTextattributes.ForegroundColor = UIColor.FromRGB(255,117,51); //Urban Science Orange
                }

                //Initialize content string for the Card's content view
                var length1 = vehicleString.Length;
                var length2 = percentileString.Length;
                var length3 = performanceString.Length;

                //Simple conversion to change font length if KPI title is too long to display
                if (length1 >= 35){
                    var fontSize = 20 - ((length1 - 35)/2);
                    vehicleTextattributes.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);
                }

                //Apply attributes to the strings
                prettyString1.SetAttributes(vehicleTextattributes.Dictionary, new NSRange(0, length1)); 
                prettyString2.SetAttributes(percentileTextattributes.Dictionary, new NSRange(0, length2)); 
                prettyString3.SetAttributes(performanceTextattributes.Dictionary, new NSRange(0, length3));

                //Append all strings to a final string to represent content view
                var prettyString = new NSMutableAttributedString() ;
                prettyString.Append(prettyString1);
                prettyString.Append(prettyString2);
                prettyString.Append(prettyString3);

                //Sets the Card's content view to attributed string
                myLabel.AttributedText = prettyString;

            }

            else
            {
                cell.TextLabel.Text = "Not Good";
            }
            return cell;
            //If table scrows out of view, a cell is unseable then cell returned as recycled cell.
            //No need for reloading etc.
        }

        //Set height for the table's cell (Not height of card)
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 156;
        }

        //Performs action once a row (table item) is selected
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            
            selectedKpi = indexedTableItems[keys[indexPath.Section]][indexPath.Row]; 
            NewPageEvent(this, new EventArgs());
        }
    }
}