using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Memberships.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Build manual string link
        /// </summary>
        /// <param name="area">The area</param>
        /// <param name="controller">The controller</param>
        /// <param name="action">The action</param>
        /// <returns>string link</returns>
        public static string BuildManualLink(string area, string controller, string action)
        {
            var link = new StringBuilder();

            if(!string.IsNullOrWhiteSpace(controller) && controller.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(area) && area.Length > 0)
                {
                    link.Append($"/{area}");
                }
                link.Append($"/{controller}");
                if (!string.IsNullOrWhiteSpace(action) && action.Length > 0)
                {
                    link.Append($"/{action}");
                }
                link.Append("/");
            }
            else
            {
                link.Append("#");
            }

            return link.ToString();
        }

        /// <summary>
        /// Build manual string link
        /// </summary>
        /// <param name="htmlHelper">The htmlHelper</param>
        /// <param name="area">The area</param>
        /// <param name="controller">The controller</param>
        /// <param name="action">The action</param>
        /// <returns>string link</returns>
        public static string BuildManualLink(this HtmlHelper htmlHelper, string area, string controller, string action)
        {
            var link = BuildManualLink(area, controller, action);
            return link;
        }


        public static TagBuilder BuildAnchorTag(string area, string controller, string action, string text, string cssClasses = "", string id = "", Dictionary<string, string> attributes = null)
        {
            //link
            var link = BuildManualLink(area, controller, action);

            //wire up anchor link
            var anchorTag = new TagBuilder("a");
            anchorTag.MergeAttribute("href", link);

            //apply any atributes passed in to the anchor tag 
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    anchorTag.MergeAttribute(attribute.Key, attribute.Value);
                }
            }

            //other wireup
            anchorTag.InnerHtml = text;
            anchorTag.AddCssClass(cssClasses);
            anchorTag.GenerateId(id);

            return anchorTag;
        }

        public static TagBuilder BuildAnchorTag(this HtmlHelper htmlHelper, string area, string controller, string action, string text, string cssClasses = "", string id = "", Dictionary<string, string> attributes = null)
        {
            var anchorTag = BuildAnchorTag(area, controller, action, text, cssClasses, id, attributes);
            return anchorTag;
        }

        /// <summary>
        /// A hepler method to to display build the glyph icon link
        /// </summary>
        /// <param name="htmlHelper">The htmlHelper</param>
        /// <param name="area">The area for the link</param>
        /// <param name="controller">The controller for the link</param>
        /// <param name="action">The action for the link</param>
        /// <param name="text">The link text</param>
        /// <param name="glyphicon">The glyph icon to use</param>
        /// <param name="cssClasses">The cssClasses to be applied</param>
        /// <param name="id">The id of the link</param>
        /// <param name="attributes">Any attributes</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BuildLink(this HtmlHelper htmlHelper, string area, string controller, string action, string text, string glyphicon, string cssClasses = "", string id = "", Dictionary<string, string> attributes = null)
        {
            // build anchor tag
            var anchorTag = BuildAnchorTag(area, controller, action, text, cssClasses, id, attributes);

            //add glyph icon
            if (!string.IsNullOrWhiteSpace(glyphicon))
            {
                var glyph = $"<span class='glyphicon glyphicon-{glyphicon}'></span>";
                anchorTag.InnerHtml = $"{glyph} {anchorTag.InnerHtml}";
            }
            
            return MvcHtmlString.Create(anchorTag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// A hepler method to to display build the link
        /// </summary>
        /// <param name="htmlHelper">The htmlHelper</param>
        /// <param name="area">The area for the link</param>
        /// <param name="controller">The controller for the link</param>
        /// <param name="action">The action for the link</param>
        /// <param name="text">The link text</param>
        /// <param name="cssClasses">The cssClasses to be applied</param>
        /// <param name="id">The id of the link</param>
        /// <param name="attributes">Any attributes</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BuildLink(this HtmlHelper htmlHelper, string area, string controller, string action, string text, string cssClasses = "", string id = "", Dictionary<string, string> attributes = null)
        {
            // build anchor tag
            var anchorTag = BuildAnchorTag(area, controller, action, text, cssClasses, id, attributes);
            return MvcHtmlString.Create(anchorTag.ToString(TagRenderMode.Normal));
        }
    }
}