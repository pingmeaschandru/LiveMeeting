namespace TW.H264Coder.Nal
{
    public enum NaluType
    {
        NaluTypeSlice = 1,      // Coded slice of a non-IDR picture
        NaluTypeDpa = 2,        // Coded slice data partition A
        NaluTypeDpb = 3,        // Coded slice data partition B
        NaluTypeDpc = 4,        // Coded slice data partition C
        NaluTypeIdr = 5,        // Coded slice of an IDR picture
        NaluTypeSei = 6,        // Supplemental enhancement information (SEI)
        NaluTypeSps = 7,        // Sequence parameter set
        NaluTypePps = 8,        // Picture parameter set
        NaluTypeAud = 9,        // Access unit delimiter
        NaluTypeEoseq = 10,     // End of sequence
        NaluTypeEostream = 11,  // End of stream
        NaluTypeFill = 12       // Filler data
    }
}