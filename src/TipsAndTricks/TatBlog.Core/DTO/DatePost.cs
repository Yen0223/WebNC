using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class DatePost
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int PostCount { get; set; }
		public string getAbbreviatedName()
		{
			DateTime date = new DateTime(2020, Month, 1);

			return date.ToString("MMM");
		}
	}
}
