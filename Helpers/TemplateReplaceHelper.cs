using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Helpers
{
    public class TemplateReplaceHelper
    {
        public static string Run(string json, string template)
        {
            var jsonObj = JObject.Parse(json);
            var nodes = new Dictionary<string, string>();

            JsonHelper.ParseToKeyValue(jsonObj, nodes);
            var formattedNodes = nodes.ToDictionary(node => node.Key, node => node.Value);

            ConditionsReplace(formattedNodes, ref template);
            LoopReplace(formattedNodes, ref template);
            BasicReplace(formattedNodes, ref template);

            return template;
        }

        private static void BasicReplace(Dictionary<string, string> nodes, ref string template)
        {
            foreach (var node in nodes)
            {
                template = template.Replace("%" + node.Key + "%", node.Value);
            }
        }

        private static void ConditionsReplace(Dictionary<string, string> nodes, ref  string template)
        {
            var regex = new Regex(@"\<if\s*{([^}\>]*)}\s+value=(\w*)\s*\>(((?!endif).)*)<\/endif\s*{([^}\>]*)}\>", RegexOptions.Singleline);
            var loopKeys = regex.Matches(template);

            foreach (Match match in loopKeys)
            {
                if (match.Groups[2]?.ToString()?.ToLower() == "true" ||
                    match.Groups[2]?.ToString()?.ToLower() == "false")
                {
                    var isParse = bool.TryParse(match.Groups[2]?.ToString()?.ToLower(), out var value);
                    var isParse2 = bool.TryParse(nodes.GetValueOrDefault(match.Groups[1]?.ToString())?.ToLower(),
                        out var keyValue);
                    
                    if (!isParse || !isParse2 || value == keyValue)
                        continue;

                }else
                {
                    if (match.Groups[2]?.ToString()?.ToLower() == nodes.GetValueOrDefault(match.Groups[1]?.ToString()))
                        continue;
                }
                
                var condition = match.Value;
                var index = template.IndexOf(condition, 0);

                template = template.Remove(index, condition.Length);
            }

        }

        private static void LoopReplace(Dictionary<string, string> nodes, ref string template)
        {
            var regex = new Regex(@"\<list\s*{(?<name>[^}\>]*)}\s*(?<number>\d*)\s*\>(((?!endlist).)*)<\/endlist\s*{([^}\>]*)}[^>]*>",  RegexOptions.Singleline);
            var loopKeys = regex.Matches(template);
            foreach (Match match in loopKeys)
            {
                var isParse = int.TryParse(match.Groups["number"]?.ToString(), out var number);
                if(!isParse) continue;
                template = LoopPartialReplace(nodes, template, match.Groups["name"]?.ToString(), number);
            }
        }
        
        private static string LoopPartialReplace(Dictionary<string, string> nodes, string template, string key, int? number)
        {
            var regex = new Regex(@"<list\s*{" + key + @"}\s*"+(number != 0 ? number.ToString() : "")+@"\s*>(.*)<\/endlist\s*{" + key + @"}\s*"+(number != 0 ? number.ToString() : "")+@"\s*>", RegexOptions.Singleline);
            var regexRequireKeys = new Regex($"{key}.*");
            var partialTemplate = regex.Match(template);
            var loopItems = partialTemplate.Groups[1].Value;

            if (loopItems.Length == 0)
            {
                return template;
            }

            var newLoopItems = ReplaceMultiItems(nodes, regexRequireKeys, loopItems);
            template = regex.Replace(template, newLoopItems);
            return template;
        }

        private static string ReplaceMultiItems(Dictionary<string, string> nodes, Regex regexRequireKeys, string loopItems)
        {
            var countItem = GetLoopItemCount(nodes, regexRequireKeys);
            var newItems = "";
            for (var i = 0; i < countItem; i++)
            {
                var partialItem = loopItems;
                foreach (var (s, value) in nodes)
                {
                    if (regexRequireKeys.IsMatch(s))
                    {
                        partialItem = partialItem.Replace("%" + s + "%", value.Split("|")[i]);
                    }
                }

                newItems += partialItem;
            }

            return newItems;
        }
        
        private static int GetLoopItemCount(Dictionary<string, string> nodes, Regex regexRequireKeys)
        {
            foreach (var node in nodes)
            {
                if (!regexRequireKeys.IsMatch(node.Key)) continue;
                return node.Value.Split("|").Count();
            }

            return 0;
        }
    }
}
