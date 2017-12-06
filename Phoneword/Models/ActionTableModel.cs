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


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        { //returned for each variable 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }


            //Sets background color to blue upon selection
            /*UIView MyView = new UIView();
            MyView.BackgroundColor = UIColor.FromRGB(61, 131, 244);
            cell.SelectedBackgroundView = MyView;*/

            //Attributes for action title string
            var titleAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.FromRGB(179,149,86),//Bronze
                Font = UIFont.FromName("HelveticaNeue-Bold", 20), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                UnderlineColor = UIColor.FromRGB(179,149,86),
                UnderlineStyle = NSUnderlineStyle.Single
            };

            //Attributes for action body string
            var bodyAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,//Black
                Font = UIFont.FromName("HelveticaNeue-Bold", 14), //All available fonts https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/

                //UnderlineColor = UIColor.FromRGB(179, 149, 86),
                //UnderlineStyle = NSUnderlineStyle.Single
            };


            //cell.TextLabel.Text = tableItems[indexPath.Row];
            if (tableItems[indexPath.Row] != null){

                /*if (indexPath.Row % 2 == 1)
                {
                    //urban science color
                    //cell.BackgroundColor = UIColor.FromRGB(117, 190, 66);
                    cell.BackgroundColor = UIColor.FromRGB(219, 241, 203); //Lightest Urban Science
                }
                else
                {
                    //lighter color
                    //cell.BackgroundColor = UIColor.FromRGB(204, 255, 153);
                    // cell.BackgroundColor = UIColor.FromRGB(204, 200, 153); //Random try for card view
                    cell.BackgroundColor = UIColor.FromRGB(220, 220, 220); //darker gray
                }
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.TextLabel.TextAlignment = UITextAlignment.Center;
                //cell.TextLabel.Font = UIFontAttributes.Bold;*/

                //Header Stuff
                //cell.ImageView.Frame 

                //border stuff
                /*cell.Layer.BorderColor = UIColor.FromRGB(204, 255, 153).CGColor; 
                cell.Layer.BorderWidth = 5;
                cell.Layer.MasksToBounds = true;*/

                //cell.Layer.CornerRadius = 6;
               /* cell.Layer.ShadowOffset = new CoreGraphics.CGSize(2, 2);
                cell.Layer.ShadowOpacity = 5f;
                cell.Layer.ShadowColor = UIColor.Black.CGColor;*/


                //Code from Card View

                // 350 is the width
                // UILabel myLabel = new UILabel(new CGRect(minX + 12, minY + 10, 350, 120));
                UILabel myLabel = new UILabel(new CGRect(12, 12, 350, 120));

                myLabel.BackgroundColor = UIColor.White;
                //myLabel.Sha

                //Make view for Selection of cell

                //Selection style
                //cell.SelectionStyle = UITableViewCellSelectionStyle.None;

                UIView MyView = new UIView();
                MyView.BackgroundColor = UIColor.White;//UIColor.FromWhiteAlpha(1.0f, 1.0f);
                myLabel.AddSubview(MyView);
                cell.SelectedBackgroundView = MyView;

                myLabel.Layer.BorderWidth = 0.8f;

                //Bronze border color
                myLabel.Layer.BorderColor = UIColor.FromRGB(179, 149, 86).CGColor;
                myLabel.Layer.CornerRadius = 4.0f;

                myLabel.Layer.MasksToBounds = false;
                //myLabel.Layer.MasksToBounds = true;

                //var shadowPath = UIBezierPath.FromRoundedRect(new CGRect(0.0f, 0.0f, 200.0f, 100.0f), 50.0f);

                //Color for shadow
                myLabel.Layer.ShadowColor = UIColor.Black.CGColor;

                //Set offset for shadow
                myLabel.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0); //Width and height

                //myLabel.MasksToBound

                myLabel.Layer.ShadowOpacity = 0.1f;
                //myLabel.ShadowOffset = new CoreGraphics.CGSize(10, 10);
                // myLabel.ShadowColor = null;
                //myLabel.Layer.ShadowPath = shadowPath.CGPath;

                //Sets the shadow upon selection, sets the inside of cell to shadowcolor
                UIBezierPath shadowPath = UIBezierPath.FromRoundedRect(myLabel.Bounds, 4.0f); //Bounds, cornerRadius
                myLabel.Layer.ShadowPath = shadowPath.CGPath;

                myLabel.Lines = 0;
                myLabel.LineBreakMode = UILineBreakMode.WordWrap;
                myLabel.TextAlignment = UITextAlignment.Center;

                // myLabel.Text = tableItems[0].actionP;

                //myView.Layer.CornerRadius = 4;
                //cell.ContentView.AddSubview(myView);

                //myLabel.Frame = CoreGraphics.CGRectEdge(myView.Bounds, 8, 8);

                cell.ContentView.AddSubview(myLabel);



                var actionTitle = "Action ";
                //cell.DetailTextLabel= rndm;
                var action = tableItems[indexPath.Row].actionP;

                var titleString = actionTitle + (indexPath.Row + 1) + " \n";

                var bodyString = "\n"+ action;
                //var strings = "ACTION 1 \n";
                //var inputs = strings + action;
                var prettyString1 = new NSMutableAttributedString(titleString);
                var prettyString2 = new NSMutableAttributedString(bodyString);

                var length1 = titleString.Length;
                var length2 = bodyString.Length;

                prettyString1.SetAttributes(titleAttributes.Dictionary, new NSRange(0, length1));
                prettyString2.SetAttributes(bodyAttributes.Dictionary, new NSRange(0, length2));


                var prettyString = new NSMutableAttributedString();
                prettyString.Append(prettyString1);
                prettyString.Append(prettyString2);
                //Can apply other attributes to the rest of the text
                //cell.TextLabel.AttributedText = prettyString;
                myLabel.AttributedText = prettyString;



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

        //Returns the height of the tablve view cell
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            //return base.GetHeightForRow(tableView, indexPath);
            return 146;
        }

        public ActionTableModel(List<KpiAction> items, ActionsViewController Owner)
        {
            tableItems = items;
            owner = Owner;
        }
    }
}
