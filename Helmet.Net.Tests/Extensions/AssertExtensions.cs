using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Helmet.Net.Tests.Extensions
{
    public static class AssertExtensions
    {
        public static async Task ThrowsAsync<TException>(Func<Task> action) where TException : Exception
        {
            var expected = typeof (TException);
            Type actual = null;

            try
            {
                await action();
            }
            catch (TException exception)
            {
                actual = exception.GetType();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}