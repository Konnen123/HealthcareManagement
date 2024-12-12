//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Options;


//namespace HealthcareManagement.IntegrationTests
//{
//    public class TestAuthenticationSchemeProvider :AuthenticationSchemeProvider
//    {
//        public const string Name = "TestAuthenticationSchemeProvider";

//        public TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options,
//            IDictionary<string,AuthenticationScheme> schemes) : base(options, schemes)
//        {

//        }

//        public override Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
//        {
//            var scheme = new AuthenticationScheme(Name, Name, typeof(TestAuthenticationHandler));
//            return Task.FromResult(scheme)!;
//        }


//    }
//}
