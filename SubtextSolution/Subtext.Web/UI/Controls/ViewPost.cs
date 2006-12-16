#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Format;
using Subtext.Framework.Tracking;
using Subtext.Web.Controls;
using Subtext.Framework.Security;
using Subtext.Extensibility.Plugins;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Control used to view a single blog post.
	/// </summary>
	public class ViewPost : BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink editLink;
		protected System.Web.UI.WebControls.HyperLink TitleUrl;
		protected System.Web.UI.WebControls.Label date;
		protected System.Web.UI.WebControls.Label commentCount;
		protected System.Web.UI.WebControls.Literal Body;
		protected System.Web.UI.WebControls.Literal PostDescription;
		protected System.Web.UI.WebControls.Literal PingBack;
		protected System.Web.UI.WebControls.Literal TrackBack;

		const string linkToComments = "<a href=\"{0}#feedback\" title=\"View and Add Comments\">{1}{2}</a>";

		/// <summary>
		/// Loads the entry specified by the URL.  If the user is an 
		/// admin and the skin supports it, will also display an edit 
		/// link that navigates to the admin section and allows the 
		/// admin to edit the post.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			//Get the entry
			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				//Raise event before any processing takes place
				SubtextEvents.OnSingleEntryRendering(entry, new SubtextEventArgs());

				BindCurrentEntryControls(entry, this);
				
				DisplayEditLink(entry);

				//Track this entry
				EntryTracker.Track(Context, entry.Id, CurrentBlog.Id);

				//Set the page title
                Globals.SetTitle(HttpUtility.HtmlEncode(entry.Title), Context);

				//Sent entry properties
                TitleUrl.Text = HttpUtility.HtmlEncode(entry.Title);
				ControlHelper.SetTitleIfNone(TitleUrl, "Title of this entry.");
				TitleUrl.NavigateUrl = entry.Url;
				Body.Text = entry.Body;
				if(PostDescription != null)
				{
					PostDescription.Text = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1}",entry.DateCreated.ToLongDateString(),entry.DateCreated.ToShortTimeString());
				}

				if(date != null)
				{
					if(date.Attributes["Format"] != null)
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateCreated.ToString(date.Attributes["Format"]));
						date.Attributes.Remove("Format");
					}
					else
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateCreated.ToString("f"));
					}
				}

				if(commentCount != null)
				{
					if(CurrentBlog.CommentsEnabled && entry.AllowComments)
					{
						if(entry.FeedBackCount == 0)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, "Add Comment", "");
						}
						else if(entry.FeedBackCount == 1)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, "One Comment", "");
						}
						else if(entry.FeedBackCount > 1)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, entry.FeedBackCount, " Comments");
						}
					}
				}
				
				//Set Pingback/Trackback 
				if(PingBack == null)
				{
					PingBack = Page.FindControl("pinbackLinkTag") as Literal;
				}
				
				if(PingBack != null)
				{
					PingBack.Text = TrackHelpers.PingPackTag;
				}
				
				if(TrackBack != null)
				{
					TrackBack.Text = TrackHelpers.TrackBackTag(entry);
				}
			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
		}

		// If the user is an admin AND the the skin 
		// contains an edit Hyperlink control, this 
		// will display the edit control.
		private void DisplayEditLink(Entry entry)
		{
			if(editLink != null)
			{
				if(SecurityHelper.IsAdmin)
				{
					editLink.Visible = true;
					if(editLink.Text.Length == 0 && editLink.ImageUrl.Length == 0)
					{
						//We'll slap on our little pencil icon.
						editLink.ImageUrl = BlogInfo.VirtualDirectoryRoot + "Images/edit.gif";
						ControlHelper.SetTitleIfNone(editLink, "Edit this entry.");
						editLink.NavigateUrl = UrlFormats.GetEditLink(entry);
					}
				}
				else
				{
					editLink.Visible = false;
				}
			}
		}

	}
}

