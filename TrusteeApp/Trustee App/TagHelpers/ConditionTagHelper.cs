using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Condition))]
    public class ConditionTagHelper : TagHelper
    {
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition) output.SuppressOutput();

            //output.Attributes.RemoveAll("bold");
            //output.PreContent.SetHtmlContent("<strong>");
            //output.PostContent.SetHtmlContent("</strong>");
        }
    }
}
