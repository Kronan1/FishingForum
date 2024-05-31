using System.Linq;
namespace FishingForum
{
    public static class Extensions
    {
        public static bool IsOnlyLetters(this string text)
        {
            if (!text.All(char.IsLetter))
            {
                return false;
            }

            return true;

        }
    }
}
