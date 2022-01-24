using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Ogsn.Utils.IO
{
    public class Csv
    {
        /// <summary>
        /// Data for read or write
        /// </summary>
        public Dictionary<string, List<string>> Data { get; set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Separator charactors array, default is ','(CSV) and '\t'(TSV)
        /// </summary>
        public char[] Separators { get; set; } = new char[] { ',', '\t' };

        /// <summary>
        /// Ignore white space in header
        /// </summary>
        public bool HeaderEmptyStringIgnoring { get; set; } = true;

        /// <summary>
        /// Distinguish between uppercase header
        /// </summary>
        public bool HeaderUppercaseDistinction { get; set; } = false;


        public bool IsEmpty => Data.Keys.Count == 0;



        public Csv() { }

        public Csv(string readFileName, string encoding = "UTF-8") => Read(readFileName, false, encoding);

        public Csv(string[] headers) => SetHeaders(headers);



        public void SetHeader(string header)
        {
            string normalized = NormalizeString(header);
            if (Data.ContainsKey(normalized))
                Data[normalized] = new List<string>();
            else
                Data.Add(normalized, new List<string>());
        }

        public void SetHeaders(params string[] headers)
        {
            foreach (var header in headers)
            {
                SetHeader(NormalizeString(header));
            }
        }

        public void AddValue(string header, object value)
        {
            header = NormalizeString(header);
            var str = value?.ToString() ?? string.Empty;
            if (Data.ContainsKey(header))
                Data[header].Add(str);
            else
                Data.Add(header, new List<string> { str });
        }





        public string GetHeader(int column)
        {
            return Data.ElementAt(column).Key;
        }


        public string[] GetHeaders()
        {
            return Data.Keys.ToArray();
        }

        public string GetValue(string header, int row)
        {
            header = NormalizeString(header);
            return Data[header].ElementAt(row);
        }

        public int GetRows(bool includHeaderRow = true)
        {
            int maxRow = Data.Values.Max(e => e.Count);
            if (includHeaderRow)
                return maxRow + 1;
            return maxRow;
        }



        public void Clear()
        {
            Data.Clear();
        }


        public void Write(string path, string separator = ",", string encoding = "UTF-8")
        {
            using var writer = new StreamWriter(path: path, append: false, encoding: Encoding.GetEncoding(encoding));

            // Write Headers
            foreach (var header in Data.Keys)
            {
                writer.Write(NormalizeString(header));
                if (!header.Equals(Data.Keys.Last()))
                    writer.Write(separator);
            }
            writer.Write(writer.NewLine);

            // Write Values
            int maxRow = Data.Values.Max(e => e.Count);
            for (int row = 0; row < maxRow; ++row)
            {
                foreach (var e in Data)
                {
                    var v = e.Value.ElementAtOrDefault(row);
                    if (string.IsNullOrEmpty(v))
                        writer.Write(string.Empty);
                    else
                        writer.Write(v);

                    if (!e.Equals(Data.Last()))
                        writer.Write(separator);
                }
                writer.Write(writer.NewLine);
            }

            writer.Flush();
        }

        public void Read(string path, bool append = false, string encoding = "UTF-8")
        {
            if (append == false)
                Clear();

            using var reader = new StreamReader(path: path, encoding: Encoding.GetEncoding(encoding));

            // Read Headers
            var headers = reader.ReadLine().Split(Separators);
            headers = headers.Select(e => NormalizeString(e)).ToArray();
            foreach (var header in headers)
            {
                SetHeader(header);
            }

            // Write Values
            while (reader.EndOfStream == false)
            {
                var values = reader.ReadLine().Split(Separators);
                for (int i = 0; i < headers.Length; ++i)
                {
                    if (i >= values.Length)
                        AddValue(headers[i], string.Empty);
                    else
                        AddValue(headers[i], values[i]);
                }
            }
        }

        string NormalizeString(string str)
        {
            if (HeaderEmptyStringIgnoring)
                str = str.Trim().Replace(" ", string.Empty);
            if (HeaderUppercaseDistinction == false)
                str = str.ToLowerInvariant();
            return str;
        }
    }
}
