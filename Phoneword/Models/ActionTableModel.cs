using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Phoneword.Models
{

    public class ActionTableModel : UITableViewSource
    {
        List<KpiAction> tableItems;
        string cellIdentifier = "TableCell";
        ActionsViewController owner;


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Count; //Number of rows in table
        }

        //GetCell method for the table to update with the appropriate content
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { 
            //Declare each cell as reusable
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }


            //Attributes for action title string
            var titleAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(179,149,86), //Bronze
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                UnderlineColor = UIColor.FromRGB(179,149,86),
                UnderlineStyle = NSUnderlineStyle.Single
            };

            //Attributes for action body string
            var bodyAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,//Black
                Font = UIFont.FromName("HelveticaNeue-Bold", 14), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

            };

            //Initializes cell content inside the table view
            if (tableItems[indexPath.Row] != null){

                //Creates a label to represent a Card in the table
                UILabel myLabel = new UILabel(new CGRect(12, 12, 350, 120));
                myLabel.BackgroundColor = UIColor.White;

                //Make the backgroundcolor of the selected table cell
                //to stay the same upon selection
                UIView MyView = new UIView();
                MyView.BackgroundColor = UIColor.White;
                myLabel.AddSubview(MyView);
                cell.SelectedBackgroundView = MyView;


                //Edit the border of the Card
                myLabel.Layer.BorderWidth = 0.8f;
                myLabel.Layer.BorderColor = UIColor.FromRGB(179, 149, 86).CGColor; //Bronze
                myLabel.Layer.CornerRadius = 4.0f;

                //Makes sure the card appears to be above the table view cell
                //Instead of on the same plane (Creates elevated effect)
                myLabel.Layer.MasksToBounds = false;

                //Color for shadow
                myLabel.Layer.ShadowColor = UIColor.Black.CGColor;

                //Set offset for shadow
                myLabel.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0); //Width and height

                //Opacity of shadow
                myLabel.Layer.ShadowOpacity = 0.1f;

                //Sets the shadow upon selection, sets the inside of cell to shadowcolor
                UIBezierPath shadowPath = UIBezierPath.FromRoundedRect(myLabel.Bounds, 4.0f); //Bounds, cornerRadius
                myLabel.Layer.ShadowPath = shadowPath.CGPath;

                //Makes sure the text does not truncate
                myLabel.Lines = 0;
                myLabel.LineBreakMode = UILineBreakMode.WordWrap;
                myLabel.TextAlignment = UITextAlignment.Center;

                //Adds Card to the content view of the cell
                cell.ContentView.AddSubview(myLabel);

                //Initialize content to be displayed by each action card
                var actionTitle = "Action ";
                var action = tableItems[indexPath.Row].actionP;
                var titleString = actionTitle + (indexPath.Row + 1) + " \n";
                var bodyString = "\n"+ action;
                var prettyString1 = new NSMutableAttributedString(titleString);
                var prettyString2 = new NSMutableAttributedString(bodyString);

                var length1 = titleString.Length;
                var length2 = bodyString.Length;

                //Applies attribute to the string
                prettyString1.SetAttributes(titleAttributes.Dictionary, new NSRange(0, length1));
                prettyString2.SetAttributes(bodyAttributes.Dictionary, new NSRange(0, length2));

                //Append individual strings to the final string that will represent the content
                var prettyString = new NSMutableAttributedString();
                prettyString.Append(prettyString1);
                prettyString.Append(prettyString2);

                //Sets the Card's value to the attributed string
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

        //Performs action once a row (table item) is selected
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            owner.setSelectedKpi(tableItems[indexPath.Row]);
            owner.url = owner.selectedAction.actionLink; //corresponding url
                
        }

        //Returns the height of the table view Action cell
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 146;
        }

        //Updates the Table's items to the items passed from selected KPI
        public ActionTableModel(List<KpiAction> items, ActionsViewController Owner)
        {
            tableItems = items;
            owner = Owner;
        }
    }
}
