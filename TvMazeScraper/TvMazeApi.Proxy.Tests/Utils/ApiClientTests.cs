using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TvMazeApi.Proxy.Tests.Common;
using TvMazeApi.Proxy.Utils;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Tests.Utils
{
    [TestFixture]
    public class ApiClientTests
    {
        #region private classes       
        public class TestData
        {
            public TestData(string value) => TestValue = value;

            public string TestValue { get; set; }

            public override bool Equals(object obj)
            {
                return ((TestData)obj).TestValue == TestValue;
            }
        }

        public class GetAsyncTestInfo
        {
            public GetAsyncTestInfo(int value, ResponseType responseType, TestData expecteData)
            {
                AttemptNumber = value;
                ExpectedAttemptNumber = value;
                ShouldTheLastAttemptBeSuccessfull = true;
                ExpectedToSeeException = false;
                ResponseType = responseType;
                ExpectedResult = expecteData;
            }

            public bool ExpectedToSeeException { get; set; }
            public int AttemptNumber { get; set; }
            public bool ShouldTheLastAttemptBeSuccessfull { get; set; }
            public int ExpectedAttemptNumber { get; set; }
            public TestData ExpectedResult { get; set; }
            public HttpStatusCode[] AllowedHttpStatuses { get; set; }
            public ResponseType ResponseType { get; set; }
        }

        public enum ResponseType
        {
            TooManyRequests,
            BadGateWay,
            Ok
        }
        #endregion

        private const string ApiUrl = "http://api_url";
        private const string QueryParams = "query_params";
        private const string TestValue = "test_value";
        private const int MaxRetryAttempts = 30;
        private const int TestDelay = 0;

        private readonly string _defaultResponseText = JsonConvert.SerializeObject(new TestData(TestValue));

        private HttpResponseMessage _defaultHttpResponse = null;
        private HttpResponseMessage _tooManyRequestsHttpResponse = null;
        private HttpResponseMessage _badGatewayHttpResponse = null;

        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IProxySettingsProvider> _proxySettingsProviderMock;
        private Mock<IThreadUtil> _threadUtilMock;

        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _defaultHttpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_defaultResponseText)
            };

            _tooManyRequestsHttpResponse = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            _badGatewayHttpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);

            _proxySettingsProviderMock = new Mock<IProxySettingsProvider>();
            _proxySettingsProviderMock.Setup(c => c.ApiUrl).Returns(ApiUrl);
            _proxySettingsProviderMock.Setup(c => c.DelayForTooManyRequestHttpError).Returns(TestDelay);
            _proxySettingsProviderMock.Setup(c => c.MaxAttemptNumberForTooManyRequestsHttpError).Returns(MaxRetryAttempts);

            _threadUtilMock = new Mock<IThreadUtil>();
            _threadUtilMock.Setup(c => c.DelayAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _apiClient = new ApiClient(_proxySettingsProviderMock.Object, _httpClientFactory.Object, _threadUtilMock.Object, _proxySettingsProviderMock.Object);
        }

        public static IEnumerable<TestCaseData> GetAsyncTestSource
        {
            get
            {
                yield return new TestCaseData(new GetAsyncTestInfo(1, ResponseType.Ok, new TestData(TestValue)))
                    .SetName("Http call. No http errors. No retry");

                yield return new TestCaseData(new GetAsyncTestInfo(1, ResponseType.BadGateWay, null)
                {
                    AllowedHttpStatuses = new[] { HttpStatusCode.BadGateway }
                })
                    .SetName("Http call. Bad request error as available error. No retry");

                yield return new TestCaseData(new GetAsyncTestInfo(1, ResponseType.BadGateWay, null)
                {
                    ExpectedToSeeException = true
                })
                    .SetName("Http call. Bad request error as unexpected error.  No retry");

                yield return new TestCaseData(new GetAsyncTestInfo(2, ResponseType.Ok, new TestData(TestValue))
                {
                    AllowedHttpStatuses = new[] { HttpStatusCode.OK }
                })
                    .SetName("Http call. No http errors. With successfull retry");

                yield return new TestCaseData(new GetAsyncTestInfo(MaxRetryAttempts + 1, ResponseType.Ok, new TestData(TestValue))
                {
                    AllowedHttpStatuses = new[] { HttpStatusCode.OK },
                    ShouldTheLastAttemptBeSuccessfull = false,
                    ExpectedAttemptNumber = MaxRetryAttempts,
                    ExpectedToSeeException = true
                })
                    .SetName("Http call. No http errors. With unsuccessfull retry");
            }
        }

        [Test]
        [TestCaseSource(nameof(GetAsyncTestSource))]
        public void GetAsyncTest(GetAsyncTestInfo testInfo)
        {
            ConfigureGetAsyncSetup(testInfo);

            if (!testInfo.ExpectedToSeeException)
            {
                var result = _apiClient.GetAsync<TestData>(QueryParams, testInfo.AllowedHttpStatuses).Result;

                Assert.AreEqual(result, testInfo.ExpectedResult);
            }
            else
            {
                Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.GetAsync<TestData>(QueryParams, testInfo.AllowedHttpStatuses));
            }

            _httpClientFactory.Verify(c => c.CreateClient(It.IsAny<string>()), Times.Exactly(testInfo.ExpectedAttemptNumber));
        }

        private void ConfigureGetAsyncSetup(GetAsyncTestInfo testInfo)
        {
            var setup = _httpClientFactory.SetupSequence(c => c.CreateClient(It.IsAny<string>()));
            for (int i = 0; i < testInfo.AttemptNumber; i++)
            {
                if (i == testInfo.AttemptNumber - 1 && testInfo.ShouldTheLastAttemptBeSuccessfull) //last scope attempt
                {
                    setup.Returns(GetHttpClient(testInfo.ResponseType));
                }
                else
                {
                    setup.Returns(GetHttpClient(ResponseType.TooManyRequests));
                }
            }
        }

        private HttpResponseMessage GetHttpResponse(ResponseType responseType)
        {
            switch (responseType)
            {
                case ResponseType.BadGateWay:
                    return _badGatewayHttpResponse;
                case ResponseType.TooManyRequests:
                    return _tooManyRequestsHttpResponse;
                case ResponseType.Ok:
                    return _defaultHttpResponse;
            }
            throw new NotSupportedException(nameof(responseType));
        }

        private HttpClient GetHttpClient(ResponseType responseType)
        {
            return new HttpClient(new FakeHttpMessageHandler(GetHttpResponse(responseType)));
        }
    }
}
