using Domain.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Use_Cases.Queries.TranslateTextQueries
{
    public class TranslateTextQuery : IRequest<Result<string>>
    {
        [JsonIgnore]
        public string? Text { get; set; }
        public string SourceLanguage { get; }
        public string TargetLanguage { get; }
        public List<string> Symptoms { get; set; }

        public TranslateTextQuery(string? text, string sourceLanguage, string targetLanguage, List<string> symptoms)
        {
            Text = text;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
            Symptoms = symptoms;
        }
    }
}
