using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMathMLParser();
        }

        #region MathMLParserTests

		static int success = 0;
        static int total = 0;

        static void TestMathMLParser()
        {
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mn>1</m:mn><m:mo>+</m:mo><m:mn>2</m:mn></m:math>", "1+2");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mrow><m:mo mathvariant=\"normal\">cos</m:mo><m:mo>⁡</m:mo><m:mrow><m:mfenced open=\"(\" close=\")\"><m:mrow><m:mn>0</m:mn></m:mrow></m:mfenced></m:mrow></m:mrow></m:math>", "(cos(((0))))");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mrow><m:mfenced open=\"(\" close=\")\"><m:mtable><m:mtr><m:mtd><m:mn>1</m:mn></m:mtd></m:mtr><m:mtr><m:mtd><m:mn>2</m:mn></m:mtd></m:mtr><m:mtr><m:mtd><m:mn>3</m:mn></m:mtd></m:mtr></m:mtable></m:mfenced></m:mrow></m:math>", "(([1;2;3]))");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mrow><m:mfenced open=\"(\" close=\")\"><m:mtable><m:mtr><m:mtd><m:mi>a</m:mi></m:mtd><m:mtd><m:mi>b</m:mi></m:mtd></m:mtr><m:mtr><m:mtd><m:mi>c</m:mi></m:mtd><m:mtd><m:mi>d</m:mi></m:mtd></m:mtr></m:mtable></m:mfenced></m:mrow></m:math>", "(([a,b;c,d]))");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mrow><m:mo>∑</m:mo><m:mrow><m:mfenced open=\"(\" close=\")\"><m:mrow><m:mi>x</m:mi></m:mrow></m:mfenced></m:mrow></m:mrow></m:math>", "(sum(((x))))");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mi>π</m:mi></m:math>", "pi");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mfenced open=\"|\" close=\"|\"><m:mrow><m:mo>-</m:mo><m:mn>1</m:mn></m:mrow></m:mfenced></m:math>", "|(-1)|");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:msup><m:mrow><m:mi>A</m:mi></m:mrow><m:mrow><m:mo>†</m:mo></m:mrow></m:msup></m:math>", "(A)'");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:mi>ϕ</m:mi></m:math>", "phi");
            Test("<?xml version=\"1.0\" encoding=\"utf-16\" ?><m:math xmlns:m=\"http://www.w3.org/1998/Math/MathML\" xmlns:MicrosoftMathRecognizer=\"http://schemas.microsoft.com/mathrecognizer\" ><m:msup><m:mrow><m:mn>3</m:mn></m:mrow><m:mrow><m:mn>2</m:mn></m:mrow></m:msup><m:mo>-</m:mo><m:msup><m:mrow><m:mn>4</m:mn></m:mrow><m:mrow><m:mn>3</m:mn><m:mo>∕</m:mo><m:mn>2</m:mn></m:mrow></m:msup></m:math>", "(3)^(2)-(4)^(3/2)");

            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
        }

        static bool Test(string xml, string query)
        {
            var parsedQuery = Sumerics.MathMLParser.Parse(xml);
            Console.WriteLine("Testing: {0} = ...", xml);
            Console.WriteLine("{0}\n-> correct: {1}", parsedQuery, query);
            return Assert(parsedQuery, query);
        }

        static bool Assert(string value, string result)
        {
            var isSuccess = true;
            total++;

            if (value == result)
            {
                success++;
                Console.WriteLine("Test successful!");
            }
            else
            {
                isSuccess = false;
                Console.WriteLine("Test failed!");
            }

            Console.WriteLine("---");
            return isSuccess;
        }

        #endregion
    }
}
