/******************************************************************************
 * PostRank 1.0
 * Copyright 2008 Declan Whelan
 * Visit http://dpwhelan.com/postrank
 * 
 * This code is relased under the Creative Commons Attribution 3.0 License
 * http://creativecommons.org/licenses/by/3.0/
 * 
 * You are free to reuse and modify this code as long as your attribte 
 * the original author: Declan Whelan http://dpwhelan.com
 *******************************************************************************/
using System;	
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using PostRank;

namespace WebApp
{
    public partial class _Default : Page
    {
        private FeedFactory feedFactory;
        private Feed currentFeed;
        private int start;

        private readonly static int Count = 10;

        private Feed CurrentFeed
        {
            get
            {
                if (currentFeed == null)
                {
                    currentFeed = feedFactory.GetFeed(urlTextBox.Text);
                }

                return currentFeed;
            }

            set { currentFeed = value; }
        }

        private Level Level
        {
            get { return TopLevelSelected() ? Level.All : (Level)Enum.Parse(typeof(Level), levelRadioButtonList.SelectedValue, true); }
        }

        private Period Period
        {
            get { return (Period)Enum.Parse(typeof(Period), periodRadioButtonList.SelectedValue, true); }
        }

        private int Start
        {
            get { return start; }
            set
            {
                start = value;
                startTextBox.Text = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            start = int.Parse(startTextBox.Text);
            feedFactory = new FeedFactory("dpwhelan.com");
            RefreshFeed();
        }

        protected void urlTextBox_TextChanged(object sender, EventArgs e)
        {
            CurrentFeed = null;
            ResetBackAndForwardButtons();
            RefreshFeed();
        }

        protected void levelRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            periodRadioButtonList.Enabled = TopLevelSelected();
            ResetBackAndForwardButtons();
            RefreshSparkline();
            RefreshPosts();
        }

        protected void periodRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPosts();
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Start = Math.Max(Start - Count, 0);
            backButton.Enabled = Start > 0;
            RefreshPosts();
        }

        protected void forwardButton_Click(object sender, EventArgs e)
        {
            Start += Count;
            backButton.Enabled = true;
            RefreshPosts();
        }

        private void ResetBackAndForwardButtons()
        {
            Start = 0;
            backButton.Enabled = Start > 0 && !TopLevelSelected();
            forwardButton.Enabled = !TopLevelSelected();
        }

        private void RefreshFeed()
        {
            titleLabel.Text = CurrentFeed.Title;
            descriptionLabel.Text = CurrentFeed.Description;

            RefreshSparkline();
            RefreshPosts();
        }

        private void RefreshSparkline()
        {
            sparkline.ImageUrl = CurrentFeed.GetSparklineUrl(Level);
        }

        private void RefreshPosts()
        {
            if (periodRadioButtonList.Enabled)
            {
                RefreshPosts(CurrentFeed.GetTopPosts(Period, 10));
            }
            else
            {
                RefreshPosts(CurrentFeed.GetPosts(Level, Count, Start));
            }
        }

        private void RefreshPosts(IEnumerable<Post> posts)
        {
            postTable.Rows.Clear();

            foreach (Post post in posts)
            {
                TableRow postRow = new TableRow();

                TableCell postRank = new TableCell();
                postRank.Text = post.PostRank.ToString("#0.0");
                postRank.Width = 32;
                postRank.BackColor = post.PostRankColor;
                postRow.Cells.Add(postRank);

                TableCell postTitle = new TableCell();
                postTitle.Text = string.Format("<a href='{0}'>{1}</a>", post.Link, post.Title);
                postTitle.ToolTip = GetPostToolTip(post);
                postRow.Cells.Add(postTitle);

                postTable.Rows.Add(postRow);
            }
        }

        private static string GetPostToolTip(Post post)
        {
            StringBuilder tooltip = new StringBuilder(post.PubDate.ToShortDateString());

            if (post.CommentCount > 0)
                tooltip.Append(String.Format(", {0} comments", post.CommentCount));

            return tooltip.ToString();
        }

        private bool TopLevelSelected()
        {
            return levelRadioButtonList.SelectedValue.Equals("Top");
        }
    }
}