using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QueueSimulator.Tests
{
    public class SlowAttribute : TestCategoryBaseAttribute
    {
        private List<string> categories = new List<string> { "Slow" };
        public override IList<string> TestCategories
        {
            get { return categories; }
        }
    }
}
