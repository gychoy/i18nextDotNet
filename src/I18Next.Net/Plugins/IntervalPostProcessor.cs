﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using I18Next.Net.Internal;

namespace I18Next.Net.Plugins
{
    public class IntervalPostProcessor : IPostProcessor
    {
        public static readonly Regex IntervalRegex = new Regex(@"\((\S*)\).*{(.*)}");

        public string IntervalSeparator { get; set; } = ";";

        public bool UseFirstAsFallback { get; set; }

        public string Keyword => "interval";

        public string Process(string key, string value, IDictionary<string, object> args)
        {
            var intervals = value.Split(IntervalSeparator);

            if (!((args?.ContainsKey("count") ?? false) && args["count"] is int count))
                count = 0;

            string found = null;
            foreach (var entry in intervals)
            {
                var match = IntervalRegex.Match(entry);

                if (match.Success && CheckIntervalMatch(match.Groups[1].Value, count))
                {
                    found = match.Groups[2].Value;
                    break;
                }
            }

            return found ?? (UseFirstAsFallback ? GetFirstMatchValue(intervals[0]) : value);
        }

        private bool CheckIntervalMatch(string value, int count)
        {
            if (value.IndexOf('-') > -1)
            {
                var parts = value.Split('-');
                int from, to;

                // Negative infinity
                if (parts[0] == "inf")
                {
                    if (!int.TryParse(parts[1], out to))
                        return false;

                    return count <= to;
                }

                // Positive ifinity
                if (parts[1] == "inf")
                {
                    if (!int.TryParse(parts[0], out from))
                        return false;

                    return count >= from;
                }

                // Both values set finite
                if (!int.TryParse(parts[0], out from) || !int.TryParse(parts[1], out to))
                    return false;

                return count >= from && count <= to;
            }

            if (int.TryParse(value, out var intervalNum))

                return intervalNum == count;

            return false;
        }

        private string GetFirstMatchValue(string interval)
        {
            var match = IntervalRegex.Match(interval);

            if (!match.Success)
                return interval;

            return match.Groups[2].Value;
        }
    }
}
