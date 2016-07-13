using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using LibNeuroOCR.Neuro;
//using LibNeuroOCR.Interface;
//using LibNeuroOCR.Exception;
//using LibNeuroOCR.Data;
using BrainNet;
using BrainNet.NeuralFramework;
using System.Collections;
using System.Threading;

namespace NeuroOCR
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Demo3();
        }
        public static void Demo1()
        {
            INeuralNetwork nn = new BackPropNetworkFactory().CreateNetwork(new ArrayList(new double[] { 2, 2, 1 }));//instantiate 2-2-1 neural network
            TrainingData[] xordata = new TrainingData[] {
                new TrainingData(new ArrayList(new double[] { 0, 0 }), new ArrayList(new double[] { 0 })),
                new TrainingData(new ArrayList(new double[] { 0, 1}), new ArrayList(new double[] {1})),
                new TrainingData(new ArrayList(new double[] { 1, 0}), new ArrayList(new double[] {1})),
                new TrainingData(new ArrayList(new double[] { 1, 1}), new ArrayList(new double[] {0}))
            }; //the xor table

            Console.WriteLine("Training the network:");
            for (int i = 0; i < 5000; i++)
            {
                foreach (TrainingData data in xordata)
                {
                    nn.TrainNetwork(data);
                }
                drawpbar(i + 1, 5000);
            }

            bool done = false;
            while (!done)
            {
                Console.WriteLine("Enter two inputs (on new lines) for the neural network! Just press enter once you are done!");
                string input = Console.ReadLine();
                if (input == "")
                {
                    done = true;
                    break;
                }
                else
                {
                    try
                    {
                        double i1 = Double.Parse(input);
                        double i2 = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Output: " + String.Join(" ", nn.RunNetwork(new ArrayList(new double[] { i1, i2 }))[0]));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("There was an error: " + Environment.NewLine + e);
                    }
                }
            }
        }
        public static void Demo2(char c)
        {
            BitArray binary = CharToBinary(c);
            Console.Write(c+" == To Binary ==> " + PrintBitArray(binary));
            Console.WriteLine(" == To Decimal ==> "+ Convert.ToInt32(PrintBitArray(binary), 2)+" == To Char ==> " + BinaryToChar(binary));
        }
        public static void Demo3()
        {
            for (int i = 48; i <= 57; i++)
            {
                Demo2((Char)i);
                Thread.Sleep(50);
            }
            for (int i = 65; i <= 90; i++)
            {
                Demo2((Char)i);
                Thread.Sleep(50);
            }
            for (int i = 97; i <= 122; i++)
            {
                Demo2((Char)i);
                Thread.Sleep(50);
            }
        }

        public static BitArray CharToBinary(char c)
        {
            if (Char.IsLetterOrDigit(c))
            {
                if (Char.IsDigit(c))
                {
                    return DecimalToBinary(Int32.Parse(c.ToString()));
                }
                else if (Char.IsLetter(c))
                {
                    if (Char.IsUpper(c))
                    {
                        return DecimalToBinary(c - 55);
                    }
                    else if (Char.IsLower(c))
                    {
                        return DecimalToBinary(c - 61);
                    }
                    else
                    {
                        throw new InvalidOperationException("The character \"" + c + "\" cannot be converted into the encoding used by this program! It also appears to be neither lowercase nor uppercase?!!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("The character \"" + c + "\" cannot be converted into the encoding used by this program! It also appears to be a letter or digit but neither a letter nor a digit at the same time?!!");
                }
            }
            else
            {
                throw new InvalidOperationException("The character \"" + c + "\" cannot be converted into the encoding used by this program!");
            }
        }

        public static char BinaryToChar(BitArray input)
        {
            int dec = Convert.ToInt32(PrintBitArray(input), 2);
            //converting to decimal ^^^

            if (dec <= 9)
            {
                //is number
                return dec.ToString()[0];
            }
            else if (dec <= 35)
            {
                //is capital
                return (Char)(dec + 55); //convert to utf because that is C#'s native encoding
            }
            else if (dec <= 61)
            {
                //is lowercase
                return (Char)(dec + 61); // ''
            }
            else
            {
                throw new InvalidOperationException("Unable to convert " + PrintBitArray(input) + " (in decimal: " + dec + ") to char!");
            }
        }

        public static string PrintBitArray(BitArray array)
        {
            string ret = "";
            foreach (bool bit in array)
            {
                if (bit) ret += "1";
                else ret += "0";
            }
            return ret;
        }

        public static BitArray DecimalToBinary(long dec)
        {
            BitArray ret = new BitArray(6,false);
            long num = dec;
            for (int i = 0; i < 6 && num > 0; i++)
            {
                if (num % 2 == 0)
                {
                    ret[ret.Count-i-1] = false;
                }
                else
                {
                    ret[ret.Count-i-1] = true;
                }
                num /= 2;
            }
            return ret;
        }

        public static BitArray RoundToBinary(params double[] input)
        {
            BitArray ret = new BitArray(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                if (Math.Round(input[i], 0, MidpointRounding.AwayFromZero) >= 1)
                {
                    ret[i] = true;
                }
                else
                {
                    ret[i] = false;
                }
            }
            return ret;
        }

        private static void drawpbar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write((((double)progress / (double)total) * 100) + "%");
            if (progress == total)
            {
                Console.Write(Environment.NewLine);
            }
        }
    }
}
