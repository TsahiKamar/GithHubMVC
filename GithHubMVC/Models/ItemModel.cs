
namespace GithHubMVC.Models
{
    public class ItemModel
    {
        public string RepositoryName { get; set; }
        public string Path { get; set; }

        //public bool? BookmarkInd { get; set; }
 
        public ItemModel(string repositoryName, string path)
        {
            RepositoryName = repositoryName;
            Path = path;
            //BookmarkInd = bookmarkInd;
         }

    }
}