using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class PostQuery
	{
		public string Keyword { get; set; }
		public string AuthorSlug { get; set; }
		public string CategorySlug { get; set; }
		public int? MonthPost { get; set; }
		public int? YearPost { get; set; }
		public string TagSlug { get; set; }
		public int Count { get; set; }
		public bool PublishedOnly { get; set; }
		public bool NotPublished { get; set; }
		public int? CategoryId { get; set; }
		public int? AuthorId { get; set; }

		public string TitleSlug { get; set; }
	}
}
