public static class TempConverter
{
    public static float[] Convert(string temp)
    {
        if (String.IsNullOrEmpty(temp))
        {
            return new float[0];
        }
        return temp.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(t => float.Parse(t)).ToArray();
    }

    public static string Convert(float[] temp)
    {
        if (temp == null)
        {
            return String.Empty;
        }
        return String.Join(';', temp);
    }
}