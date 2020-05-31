namespace VisitCardApp.Helpers
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Text;
    using System.Text.Encodings.Web;

    public static class HtmlPaginationHelper
    {
        // 1...6 7 (8) 9 10...end
        public static HtmlString Pagination(this IHtmlHelper htmlHelper, PaginationModel pagination, Func<int, string> pageUrl)
        {
            int pagelinkVisibleLeft = 3;
            int pagelinkVisibleRight = 3;

            pagination.Page = pagination.Page < 1 ? 1 : pagination.Page;

            int startIndex = 1;
            int endIndex = (pagination.Page + pagelinkVisibleRight) > pagination.TotalPages ?
                pagination.TotalPages :
                pagination.Page + pagelinkVisibleRight;

            TagBuilder tag = new TagBuilder("nav");
            tag.MergeAttribute("aria-label", "Page navigation");

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination justify-content-end");

            if (pagination.Page > pagelinkVisibleLeft)
            {
                startIndex = pagination.Page - pagelinkVisibleLeft;

                TagBuilder liTag = new TagBuilder("li");
                liTag.AddCssClass("page-item");
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", pageUrl(1));
                aTag.AddCssClass("page-link");
                aTag.InnerHtml.Append("1");
                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);

                TagBuilder li2Tag = new TagBuilder("li");
                li2Tag.AddCssClass("page-item disabled");
                TagBuilder a2Tag = new TagBuilder("a");
                a2Tag.AddCssClass("page-link");
                a2Tag.InnerHtml.Append("...");
                li2Tag.InnerHtml.AppendHtml(a2Tag);
                ulTag.InnerHtml.AppendHtml(li2Tag);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                TagBuilder liTag = new TagBuilder("li");
                liTag.AddCssClass("page-item");

                if (pagination.Page == i)
                {
                    liTag.AddCssClass("active");
                    TagBuilder aTag = new TagBuilder("span");
                    aTag.AddCssClass("page-link");
                    aTag.InnerHtml.Append(i.ToString());
                    liTag.InnerHtml.AppendHtml(aTag);
                }
                else
                {
                    TagBuilder aTag = new TagBuilder("a");
                    aTag.MergeAttribute("href", pageUrl(i));
                    aTag.AddCssClass("page-link");
                    aTag.InnerHtml.Append(i.ToString());
                    liTag.InnerHtml.AppendHtml(aTag);
                }

                ulTag.InnerHtml.AppendHtml(liTag);
            }

            if ((pagination.Page + pagelinkVisibleRight) < pagination.TotalPages)
            {
                TagBuilder li2Tag = new TagBuilder("li");
                li2Tag.AddCssClass("page-item disabled");
                TagBuilder a2Tag = new TagBuilder("a");
                a2Tag.AddCssClass("page-link");
                a2Tag.InnerHtml.Append("...");
                li2Tag.InnerHtml.AppendHtml(a2Tag);
                ulTag.InnerHtml.AppendHtml(li2Tag);

                TagBuilder liTag = new TagBuilder("li");
                liTag.AddCssClass("page-item");
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", pageUrl(pagination.TotalPages));
                aTag.AddCssClass("page-link");
                aTag.InnerHtml.Append(pagination.TotalPages.ToString());
                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }

            tag.InnerHtml.AppendHtml(ulTag);

            var writer = new System.IO.StringWriter();
            tag.WriteTo(writer, HtmlEncoder.Default);

            return new HtmlString(writer.ToString());
        }
    }
}

/*
 <nav aria-label="Page navigation example">
  <ul class="pagination justify-content-end">
    <li class="page-item disabled">
      <a class="page-link" href="#" tabindex="-1">Previous</a>
    </li>
    <li class="page-item"><a class="page-link" href="#">1</a></li>
    <li class="page-item"><a class="page-link" href="#">2</a></li>
    <li class="page-item"><a class="page-link" href="#">3</a></li>
    <li class="page-item">
      <a class="page-link" href="#">Next</a>
    </li>
  </ul>
</nav>


    <span class="page-link">
        2
        <span class="sr-only">(current)</span>
      </span>

     */
