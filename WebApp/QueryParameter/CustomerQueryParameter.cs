using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.QueryParameter
{
    public class CustomerQueryParameter
    {
        private int maxPageCount = 100;
        public int Page { get; set; } = 1;

        private int _PageCoont = 100;
        public int PageCount
        {
            get {
                return _PageCoont;
            }
            set {
                _PageCoont = (value > maxPageCount) ? maxPageCount : value;
            }
        }

        public bool HasQuery {
            get
            {
                return !String.IsNullOrEmpty(Query);
            }
        }

        public string Query { get; set; }

        public string OrderBy { get; set; } = "Firstname";
        public bool Descending
        {
            get
            {
                if (!String.IsNullOrEmpty(OrderBy))
                {
                    return OrderBy.Split(" ").Last().ToLowerInvariant().StartsWith("desc");
                }

                return false;
            }
        }
    }
}
