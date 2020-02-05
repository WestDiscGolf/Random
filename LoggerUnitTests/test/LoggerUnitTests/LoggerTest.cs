using System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LoggerUnitTests
{
    public class LoggerTest
    {
        [Fact]
        public void VerifyWasCalled()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMe();

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [Fact]
        public void VerifyWasCalledWithExpectedValues()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMe();

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Debug),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [Fact]
        public void VerifyWasCalledWithMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);
            
            // Act
            sut.RunMe();

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Debug),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Logging this ..."),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [Fact]
        public void VerifyWasCalledWithReusableExtension()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMe();

            // Assert
            loggerMock.VerifyDebugWasCalled();
        }

        [Fact]
        public void VerifyWasCalledWithReusableExtensionWithMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMe();

            // Assert
            loggerMock.VerifyDebugWasCalled("Logging this ...");
        }

        [Fact]
        public void VerifyLoopTest_1()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMeLoop();

            // Assert
            loggerMock.VerifyDebugWasCalled("Logging Multiple Times ...")
                      .VerifyDebugWasCalled("Logging Multiple Times ...")
                      .VerifyDebugWasCalled("Logging Multiple Times ...");
        }

        [Fact]
        public void VerifyLoopTest_2()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMeLoop();

            // Assert
            loggerMock.VerifyLogging("Logging Multiple Times ...", LogLevel.Debug, Times.Exactly(3));
        }

        [Fact]
        public void VerifyMultiDebug()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMeMultiDebug();

            // Assert
            //loggerMock.VerifyLogging("Message one.", LogLevel.Debug, Times.Once())
            //    .VerifyLogging("Message two.", LogLevel.Debug, Times.Exactly(1))
            //    .VerifyLogging("Message three.", LogLevel.Debug, Times.Exactly(1));
            
            loggerMock.VerifyLogging("Message one.")
                .VerifyLogging("Message two.")
                .VerifyLogging("Message three.");
        }

        [Fact]
        public void VerifyDifferentLevels()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PleaseTestMe>>();
            var sut = new PleaseTestMe(loggerMock.Object);

            // Act
            sut.RunMeDifferentLevels();

            // Assert
            loggerMock.VerifyLogging("Message one.", LogLevel.Debug, Times.Once())
                .VerifyLogging("Message two.", LogLevel.Warning, Times.Once())
                .VerifyLogging("Message three.", LogLevel.Error, Times.Once());
        }
    }
}
