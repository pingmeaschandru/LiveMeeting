namespace TW.H264Coder.Vcl.Entropy
{
    public class SyntaxElement
    {
        public int Type { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Length { get; set; }
        public int Info { get; set; }
        public int Bitpattern { get; set; }
        public int Context { get; set; }
    }
}