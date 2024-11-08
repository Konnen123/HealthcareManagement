
namespace Application.Utils
{
    public static class GuidChecker
    {
        public static bool BeAValidGuid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
