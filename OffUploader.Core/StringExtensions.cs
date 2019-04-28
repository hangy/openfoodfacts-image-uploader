namespace OffUploader.Core
{
    using Nager.ArticleNumber;

    internal static class StringExtensions
    {
        public static bool IsValidCode(this string value)
        {
            var articleNumberType = ArticleNumberHelper.GetArticleNumberType(value);
            return articleNumberType == ArticleNumberType.EAN8 ||
                articleNumberType == ArticleNumberType.EAN13 ||
                articleNumberType == ArticleNumberType.UPC ||
                articleNumberType == ArticleNumberType.GTIN;
        }
    }
}
