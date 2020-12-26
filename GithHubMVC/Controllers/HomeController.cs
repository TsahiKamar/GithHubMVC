using GithHubMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GithHubMVC.Controllers
{
    public class HomeController : Controller
    {
        #region Private
        private static readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();
        private static List<Item> tempItems = null;
        private static List<ItemModel> sessions = new List<ItemModel>();

        #endregion
         
        #region Public
        public ActionResult Index()
        {
             IList<Item> itemList = new List<Item>();
             return View(itemList);
        }

         public ActionResult Back()
        {
            return View("Index", tempItems);
        }

        [HttpGet]
        public ActionResult SessionSave(string RepositoryName,string Path) 
        {
            try
            {
                if ( ! sessions.Exists(x => x.RepositoryName == RepositoryName))
                {
                    Session["RepositoryName"] = RepositoryName;
                    Session["Path"] = Path;
                    sessions.Add(new ItemModel(RepositoryName = Session["RepositoryName"].ToString(), Path = Session["Path"].ToString()));
                }
            }
            catch (Exception ex)
            {
                string s = String.Format("RepositoryName: {0}, Path: {1}", RepositoryName, Path);
                EventLog.WriteEntry("SessionSave error : ", ex.ToString() + Environment.NewLine + "Params => " + s, EventLogEntryType.Error);
            }

            return View("Index", tempItems);
        }

        [HttpGet]
        public ActionResult GetBookmarks()  
        {
            string str = null;

            try
            {
                foreach (string key in Session.Keys)
                {
                    str += string.Format("{0}: {1}<br />", key, Session[key].ToString());
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("GetBookmarks error : ", ex.ToString(), EventLogEntryType.Error);

            }
            return View("Session", sessions);
        }

        [HttpGet]
        public async Task<ActionResult> Search(string repositoryName) 
        {
            string url = "https://api.github.com/search/repositories?q=";//YOUR_SEARCH_KEYWORD
            if (repositoryName == null || repositoryName == "") repositoryName = "YOUR_SEARCH_KEYWORD";//Default search value

            try
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# App");
                using (var result = await _httpClient.GetAsync($"{url}{repositoryName}"))
                {
                    string content = await result.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ResponseModel>(content);
                      
                    tempItems = obj.items;

                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Search error : ", ex.ToString(), EventLogEntryType.Error);
            }
            return View("Index", tempItems);

        }

        public ActionResult GetContact()
        {
            return View("Contact");
        }


        #endregion

    }
}
