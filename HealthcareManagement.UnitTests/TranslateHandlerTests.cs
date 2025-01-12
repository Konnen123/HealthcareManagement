using Amazon.Translate;
using Amazon.Translate.Model;
using Application.Use_Cases.Queries.TranslateTextQueries;
using Application.Use_Cases.QueryHandlers.TranslateTextQueryHandlers;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace HealthcareManagement.UnitTests
{
    public class TranslateHandlerTests
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonTranslate _translateClient;
        private readonly TranslateTextQueryHandler _handler;

        public TranslateHandlerTests()
        {
            _translateClient = Substitute.For<IAmazonTranslate>();
            _handler = new TranslateTextQueryHandler(_translateClient);
        }

        [Fact]
        public async Task Handle_TextIsNull_ReturnsFailure()
        {
            var query = new TranslateTextQuery(null, "en", "es", new List<string>());
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Text to translate is null", result.Error.Code);
        }

        [Fact]
        public async Task Handle_SourceAndTargetLanguagesAreSame_ReturnsSuccessWithSameText()
        {
            var query = new TranslateTextQuery("Hello", "en", "en", new List<string>());
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Hello", result.Value);
        }

        [Fact]
        public async Task Handle_TranslationSucceeds_ReturnsTranslatedText()
        {
            var query = new TranslateTextQuery("Hello", "en", "es", new List<string>());
            var translateTextResponse = new TranslateTextResponse { TranslatedText = "Hola" };

            _translateClient.TranslateTextAsync(Arg.Any<TranslateTextRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(translateTextResponse));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Hola", result.Value);
        }

        [Fact]
        public async Task Handle_TranslationFails_ReturnsFailure()
        {
            var query = new TranslateTextQuery("Hello", "en", "es", new List<string>());

            _translateClient.TranslateTextAsync(Arg.Any<TranslateTextRequest>(), Arg.Any<CancellationToken>())
                .Throws(new Exception("Translation error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Translation error", result.Error.Code);
        }
    }
}
