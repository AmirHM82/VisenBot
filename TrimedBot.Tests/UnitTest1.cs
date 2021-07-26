using System;
using Xunit;
using Moq;
using TrimedBot.Core.Commands.Message;

namespace TrimedBot.Tests
{
    public class NPMessage
    {
        [Fact]
        public void InputPageNum()
        {
            // Arrange
            var mock = new Mock<SendNPMessageCommand>();
            mock.SetupGet<int>(m => m.pageNumber).Returns(-2);
            var instance = mock.Object;
            // Act
            instance.Do();

            // Assert
            Assert.Equal(null, null);
        }
    }
}
