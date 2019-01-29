
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Solution
{



    // Complete the maxTickets function below.
    static int maxTickets(List<int> tickets)
    {
        var tempMat = new List<int>();
        for (int i = 0; i < tickets.Count; i++)
        {
            for (int j = i; j < tickets.Count; j++)
            {
                if (tickets[j] < tickets[i])
                {
                    var t = tickets[i];
                    tickets[i] = tickets[j];
                    tickets[j] = t;
                }
            }
        }
        int maxLen = 0;
        int tempLen = 1;
        for (int i = 0; i < tickets.Count - 1; i++)
        {
            if (tickets[i] == tickets[i + 1] || tickets[i] + 1 == tickets[i + 1])
            {
                //if (tickets[i] != tickets[i + 1])
                tempLen += 1;
            }
            else
            {
                if (maxLen <= tempLen)
                {
                    maxLen = tempLen;
                    tempLen = 1;
                }
            }
        }
        if (tempLen > maxLen)
            maxLen = tempLen;
        return maxLen;
    }

    static void Main(string[] args)
    {
        //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        //int ticketsCount = Convert.ToInt32(Console.ReadLine().Trim());

        List<int> tickets = new List<int>();

        //for (int i = 0; i < ticketsCount; i++)
        //{
        //    int ticketsItem = Convert.ToInt32(Console.ReadLine().Trim());
        //    tickets.Add(ticketsItem);
        //}

        tickets = new List<int> { 4,4,3,1,2,3 };
        int res = maxTickets(tickets);

        //textWriter.WriteLine(res);

        //textWriter.Flush();
        //textWriter.Close();
        Console.Read();
    }
}
