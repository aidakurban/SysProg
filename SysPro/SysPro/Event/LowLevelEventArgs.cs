namespace SysPro
{
    public class LowLevelEventArgs
    {
        public string InputValue1 { get; set; }
        public string InputValue2 { get; set; }

        public LowLevelEventArgs(string inputValue1, string inputValue2)
        {
            InputValue1 = inputValue1;
            InputValue2 = inputValue2;
        }

    }
}