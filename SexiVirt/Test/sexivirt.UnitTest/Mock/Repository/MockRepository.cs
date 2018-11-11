using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sexivirt.Model;

namespace sexivirt.UnitTest.Mock.Repository
{
    public partial class MockRepository : Mock<IRepository>
    {
        public MockRepository(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            GenerateRoles();
            GenerateUsers();
        }
    }
}
