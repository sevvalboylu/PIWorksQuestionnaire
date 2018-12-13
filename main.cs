
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication10
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = @"C:\Users\User\Downloads\exhibitA-input.csv";

            // inputTable contains the values read from csv file  
            List<string[]> inputTable = new List<string[]>();
            using (var reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] vals = line.Split('\t');
                    inputTable.Add(vals);
                }
            }

            int clientCount = 0;
            // now querying for the desired output --> 10/08/2016  
            // Fields: PLAY_ID SONG_ID CLIENT_ID PLAY_TS  
            List<string[]> outputTable = new List<string[]>();
            int len = inputTable.Count;
            for (int i = 1; i < len; i++)
            {
                string play_ts = inputTable[i].GetValue(3).ToString();
                string[] dateTime = play_ts.Split(' ');
                if (dateTime[0] == "10/08/2016")
                {
                    outputTable.Add(inputTable[i]);
                    string client_no = inputTable[i].GetValue(2).ToString();
                    int client_nr = Int32.Parse(client_no);

                    // Needed for array init
                    if (client_nr > clientCount)
                        clientCount = client_nr;
                }
            }
                 
            //string[][] count = new string[clientCount][]; // I am aware this may be extremely inefficient but faster
            List<string>[] count = new List<string>[clientCount+1];

            int UPPER = outputTable.Count;
            for (int i = 0; i < UPPER; i++)
            {
                string[] item = outputTable[i];
                int index = Int32.Parse(item.GetValue(2).ToString());
                if (count[index] == null)
                    count[index] = new List<string>();
                count[index].Add(item[1].ToString());
            }

            // now another list to store the number of distinct songs played by users
            List<int> distinct = new List<int>();
            for (int i = 0; i < clientCount; i++)
            {
                if (count[i] != null)
                    distinct.Add(count[i].Distinct().Count());
            }

            List<int[]> final = new List<int[]>();
            while (distinct.Count > 0)
            {
                int pivot = distinct[0];

                List<int> j = distinct.FindAll(x => x == pivot);
                int[] temp = new int[2];
                temp[0] = pivot; 
                temp[1] = j.Count;
                final.Add(temp);
                distinct.RemoveAll(x => x == pivot);

            }
            // Now writing to csv file
            var csv = new StringBuilder();

            for (int i = 0; i < final.Count; i++)
            {
                var first = final[i][0].ToString();
                var second = final[i][1].ToString();
                var newLine = string.Format("{0},{1}", first, second);
                csv.AppendLine(newLine);
            }

            string outFilePath = @"C:\Users\User\Downloads\exhibitA-output.csv";
            File.WriteAllText(outFilePath, csv.ToString());
        }
    }
}
