using Amazon.Translate;
using Amazon.Translate.Model;
using Application.Use_Cases.Queries.TranslateTextQueries;
using Domain.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Use_Cases.QueryHandlers.TranslateTextQueryHandlers
{
    public class TranslateTextQueryHandler : IRequestHandler<TranslateTextQuery, Result<string>>
    {
        private readonly IAmazonTranslate translateClient;

        public TranslateTextQueryHandler(IAmazonTranslate translateClient)
        {
            this.translateClient = translateClient;
        }

        public async Task<Result<string>> Handle(TranslateTextQuery request, CancellationToken cancellationToken)
        {
            if(request.Text == null)
            {
                return Result<string>.Failure(new Error("Text to translate is null"));
            }

            if (request.SourceLanguage == request.TargetLanguage)
            {
                return Result<string>.Success(request.Text);
            }

            var translateTextRequest = new TranslateTextRequest
            {
                SourceLanguageCode = request.SourceLanguage,
                TargetLanguageCode = request.TargetLanguage,
                Text = request.Text
            };

            try
            {
                var translateTextResponse = await translateClient.TranslateTextAsync(translateTextRequest);
                return Result<string>.Success(translateTextResponse.TranslatedText);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(new Error(e.Message));
            }

        }
    }
}
