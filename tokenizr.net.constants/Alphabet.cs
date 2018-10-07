namespace tokenizr.net.constants
{
  public class Alphabet
  {
    public const string English = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string EnglishPunctuation = "\"':;,.?-!()";
    public const string EnglishSpecialCharacters = "~`@#$%^&*_+={}[]/\\|<>£";
    public const string EnglishWithPunctuation = English + EnglishPunctuation;
    public const string EnglishWithPunctuationAndSpecialCharacters = EnglishWithPunctuation + EnglishSpecialCharacters;
  }
}
