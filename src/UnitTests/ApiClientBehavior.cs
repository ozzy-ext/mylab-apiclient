using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using MyLab.ApiClient;
using Xunit;

namespace UnitTests
{
    public class ApiClientBehavior
    {
        [Fact]
        public void ShouldFailWhenExpressionIsNotMethodCall()
        {
            //Arrange
            var httpClientProvider = new Mock<IHttpClientProvider>().Object;
            var client = new ApiClient<IContract>(httpClientProvider);
            
            //Act & Assert
            Assert.Throws<NotSupportedException>(() => client.Request(c => Task.CompletedTask));
        }
        
        [Fact]
        public void ShouldPassWhenExpressionIsMethodCall()
        {
            //Arrange
            var httpClientProvider = new Mock<IHttpClientProvider>().Object;
            var client = new ApiClient<IContract>(httpClientProvider);
            
            //Act & Assert
            client.Request(c => c.Foo());
        }

        [Api]
        interface IContract
        {
            [Get]
            Task<int> Foo();
        }
    }
}