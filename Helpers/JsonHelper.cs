using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Helpers
{
    public class JsonHelper
    {
        public static void ParseToKeyValue(JToken token, Dictionary<string, string> nodes, string parentLocation = "")
        {
            if (token.HasValues)
            {
                foreach (var child in token.Children())
                {
                    if (token.Type == JTokenType.Property)
                    {
                        if (parentLocation == "")
                        {
                            parentLocation = ((JProperty)token).Name;
                        }
                        else
                        {
                            parentLocation += "." + ((JProperty)token).Name;
                        }
                    }

                    ParseToKeyValue(child, nodes, parentLocation);
                }

            }
            
            if (nodes.ContainsKey(parentLocation))
            {
                nodes[parentLocation] += "|" + token.ToString();
            }
            else
            {
                nodes.Add(parentLocation, token.ToString());
            }

        }
    }
}
