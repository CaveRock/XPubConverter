using NBitcoin.DataEncoders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XPubConverter {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            string electrumTestYPub = "vpub5VLSyqLKYmnnQM2KPtJQ4cFxaEEarLyDdbvAPZCMT8LvNqMQnuQAXrFPrxQ7s4fMe9gtvpYkdRvLpZRAHUq9B87komwhX7bXrSMmXz7n8iV";
            Console.WriteLine("Orig: " + electrumTestYPub);
            Console.WriteLine("");

            //Result taken from https://jlopp.github.io/xpub-converter/
            string expectedResult = "xpub68zybAg9r8sk6wQZ4bseUnSxvAXUjaxDTpxbxLz8D96gV2ysJDjHmyZfuNKHrsyCTRvWRmjsYkdSb8eHisfAmbUxYTLYgwEWPtV4K85ch6v";
            Console.WriteLine("Correct: " + expectedResult);
            Console.WriteLine("  Wrong: " + ChangeVersionBytes(electrumTestYPub, "xpub"));
            Console.WriteLine("");
            Console.WriteLine("Last 5 characters are different, checksum?");
        }

        public static string ChangeVersionBytes(string xpub, string targetFormat) {

            initialisePrefixes();
            if (!prefixes.ContainsKey(targetFormat)) {
                throw new Exception("Invalid target version");
            }

            //trim whitespace
            xpub = xpub.Trim();

            try {
                var data = Encoders.Base58.DecodeData(xpub);
                //Version bytes
                byte[] data2 = data.Skip(4).ToArray();

                //Version prefix for target
                string prefix = prefixes[targetFormat];

                //Convert hex to bytes
                byte[] hexPrefix = Encoders.Hex.DecodeData(prefix);

                //combine new version bytes to key bytes
                byte[] result = Combine(hexPrefix, data2);

                //byte[] checkSum = Hashes.SHA256(data);
                //checkSum = Hashes.SHA256(checkSum);

                //string strCheckSum = Encoders.Base58.EncodeData(checkSum.Take(4).ToArray());
                //result = Combine(result, checkSum.Take(4).ToArray());

                //Encode to base58
                return Encoders.Base58.EncodeData(result);

            } catch (Exception ex) {
                throw new Exception("Invalid extended public key! Please double check that you didn't accidentally paste extra data.");
            }
        }

        public static byte[] Combine(byte[] first, byte[] second) {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private static Dictionary<string, string> prefixes;
        private static void initialisePrefixes() {
            prefixes = new Dictionary<string, string>();
            prefixes.Add("xpub", "0488b21e");
            prefixes.Add("ypub", "049d7cb2");
            prefixes.Add("Ypub", "0295b43f");
            prefixes.Add("zpub", "04b24746");
            prefixes.Add("Zpub", "02aa7ed3");
            prefixes.Add("tpub", "043587cf");
            prefixes.Add("upub", "044a5262");
            prefixes.Add("Upub", "024289ef");
            prefixes.Add("vpub", "045f1cf6");
            prefixes.Add("Vpub", "02575483");
        }
    }
}
