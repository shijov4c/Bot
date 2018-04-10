using Microsoft.Azure.Search.Models;

namespace PictureBot.Models
{
    public class ImageMapper 
    {
        public static SearchHit ToSearchHit(SearchResult hit)
        {
            var searchHit = new SearchHit
            {
                Key = (string)hit.Document["id"],
                Title = (string)hit.Document["businessname"]
            };

            return searchHit;
        }

    }
}