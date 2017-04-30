namespace SlExport
{
  public enum LangStatus
  {
    Undefined = -1,
    // "not translated" will not appear in the Sisulizer file. If no translation exists, the line is omitted from the XML
    NotTranslated = 0,
    BestGuess = 1,
    AutoTranslated = 2,
    Translated = 3,
    ForReview = 4,
    // "Completed" will not appear in the Sisulizer file, because it is the default. If a line has not status, "Completed" is assumed
    Completed = 5,
  }
}