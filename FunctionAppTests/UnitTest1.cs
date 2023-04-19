using FunctionApp1;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Moq;
using System.Net;
using static FunctionApp1.PublicApiFunction;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace FunctionAppTests
{
    public class UnitTest1
    {
        [Fact]
        public async void Run_ShouldAddDataToTable_WhenEverythingPasses()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new ByteArrayContent(new byte[] { 1, 2, 3 });
            responseMessage.Content = content;

            var tableCollectorMock = new Mock<IAsyncCollector<PublicApi>>();
            var binderMock = new Mock<IBinder>();
            var outputBlobMock = new Mock<Stream>();
            var loggerMock = new Mock<ILogger>();
            // Make this mocking work
            var timerInfoMock = new Mock<TimerInfo>();
            timerInfoMock.SetupGet(x => x.IsPastDue).Returns(false);
            timerInfoMock.SetupGet(x => x.ScheduleStatus).Returns(new ScheduleStatus());

            var function = new PublicApiFunction();

            // Create a fake HttpRequest object
            var httpRequest = new DefaultHttpRequest(new DefaultHttpContext());

            // Act
            await function.Run(timerInfoMock.Object, tableCollectorMock.Object, binderMock.Object, loggerMock.Object);

            // Assert
            tableCollectorMock.Verify(x => x.AddAsync(It.Is<PublicApi>(y => y.IsSuccessful == true), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}