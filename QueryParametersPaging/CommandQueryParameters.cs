using System.Linq;

namespace WebApiTest.QueryParametersPaging
{
    //This calss is for paging - if I want to see the content 
    //use in URL - URL?page=1&pagecount=3
    public class CommandQueryParameters
    {
        private const int maxPageCount = 100;
        public int Page { get; set; } = 1;
        private int _pageCount = 100;
        public int PageCount 
        { 
            get
            {
                return _pageCount;
            }
            set
            {
                _pageCount = (value > maxPageCount) ? maxPageCount : value;
            } 
        }

        //for filtering
        //http://localhost:5000/api/commands?query=Create a new test
        //to filet use - ?query=parameter
        //can also use pagination and filter together
        //?query=parameter&page=1&pagecount=3
        public bool HasQuery { get { return !string.IsNullOrEmpty(Query); } }
        public string Query { get; set; }


        //for ordering
        //?orderby=line or ?orderby=how to or ?orderby=line desc
        public string OrderBy { get; set; } = "HowTo";
        public bool Descending
        {
            get
            {
                if(!string.IsNullOrEmpty(OrderBy))
                {
                    return OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
                }
                return false;
            }
        }
    }
}