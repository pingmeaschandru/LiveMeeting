using System.Collections.Generic;

namespace TW.Coder.Converter
{
    public static class ConverterFactory
    {
        private static readonly Dictionary<string, ConverterBase> ConverterPool;

        static ConverterFactory()
        {
            //TODO : read the below from XML file
            ConverterPool = new Dictionary<string, ConverterBase>
                                {
                                    {"RGB24#YUV420", new RGB24ToYUV420Converter()},
                                    {"RGB24#YUV444", new RGB24ToYUV444Converter()},
                                    {"YUV420#RGB24", new YUV420ToRGB24Converter()},
                                    {"YUV444#RGB24", new YUV444ToRGB24Converter()}
                                };
            
        }

        public static ConverterBase CreateConverter(string sourceFormat, string destinationFormat)
        {
            ConverterBase converter;
            return ConverterPool.TryGetValue(sourceFormat + "#" + destinationFormat, out converter) ? converter : null;
        }
    }
}