using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Tests
{
    public class TestHostFixture : IDisposable
    {
        public TestHostFixture() { 
         // Host is built automatically by static ctor in TestHost
        }
        public void Dispose() { 
            // Dispose once after all tests in the collection
            TestHost.Dispose(); 
        }
    }
}
