using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsServiceSample.Services
{
    public interface ITestService
    {
        void TestMethod();
    }

    public  class TestService : ITestService
    {

        public void TestMethod()
        {
            Console.WriteLine("Test method has been executed");
        }
    }
}
