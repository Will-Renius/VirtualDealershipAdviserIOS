using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

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


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

            //Attributes for action title string
            var attributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("Courier-Bold", 24), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/


                UnderlineStyle = NSUnderlineStyle.Single
            };

            //cell.TextLabel.Text = tableItems[indexPath.Row];
            if (tableItems[indexPath.Row] != null){

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

                var actionTitle = "Action ";
                //cell.DetailTextLabel= rndm;
                var action = tableItems[indexPath.Row].actionP;

                var finalString = actionTitle + (indexPath.Row + 1) + " \n" + action;
                //var strings = "ACTION 1 \n";
                //var inputs = strings + action;
                var prettyString = new NSMutableAttributedString(finalString);

                var endRange = 9;

                if(indexPath.Row >= 9)
                {
                    endRange = 10;
                }

                prettyString.SetAttributes(attributes.Dictionary, new NSRange(0, endRange));

                //Can apply other attributes to the rest of the text
                cell.TextLabel.AttributedText = prettyString;



                //cell.TextLabel.Text = tableItems[indexPath.Row].actionP;
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
            owner.url = owner.selectedAction.actionLink; //corresponding url
                
            //Harry: Page halts, doesn't let you pick different action rows like you can with kpi
            //Error: System.NullReferenceException: Object reference not set to an instance of an object

            //Dan, Fixed it, the owner value was not added to the TableActionModel before
        }


        public ActionTableModel(List<KpiAction> items, ActionsViewController Owner)
        {
            tableItems = items;
            owner = Owner;
        }
    }
}
