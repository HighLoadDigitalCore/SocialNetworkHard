using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace sexivirt.UnitTest.Mock.Http
{
    public class MockHttpResponse : Mock<HttpResponseBase>
    {
        public MockHttpResponse(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {

        }
    }
}
