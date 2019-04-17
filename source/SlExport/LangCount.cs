namespace SlExport
{
  public class LangCount
  {
    public int StringCount { get; set; }
    public int InvalidStringCount { get; set; }
    public int WordCount { get; set; }
    public int InvalidWordCount { get; set; }

    public void Increment(bool isValid, int stringCount, int wordCount)
    {
      if (isValid)
      {
        this.StringCount += stringCount;
        this.WordCount += wordCount;
      }
      else
      {
        this.InvalidStringCount += stringCount;
        this.InvalidWordCount += wordCount;
      }
    }
  }
}