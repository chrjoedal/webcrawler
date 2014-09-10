﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.ReadKey();
        }

        private static List<robotRestriction> getRobotsRestrictions(Uri webPage, string botName, List<webPageDelays> webDelays)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            string robotFile = domainName +"/robots.txt";
            int domainHash = domainName.GetHashCode();
            List<robotRestriction> robList = new List<robotRestriction>();
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(robotFile);
            StreamReader reader = new StreamReader(stream);
            string str = "";
            string agent = "*";
            bool hasSpecificRules = false;
            bool isRelevant = true;
            botName = botName.ToLower();

            while (reader.Peek() >= 0)
            {
                str = reader.ReadLine().ToLower();

                if (str.StartsWith("user-agent:"))
                {
                    str = str.Substring("user-agent:".Length);
                    str = str.Trim();
                    agent = str;
                    if (str == botName)
                    {
                        if (!hasSpecificRules)
                        {
                            robList.Clear();
                            hasSpecificRules = true;
                            isRelevant = true;
                        }
                    } else if (str == "*")
                    {
                        if(!hasSpecificRules)
                        {
                            isRelevant = true;
                        }
                        else
                        {
                            isRelevant = false;
                        }
                    } else {
                        isRelevant = false;
                    }
                }
                if(isRelevant) 
                {
                    if (str.StartsWith("allow"))
                    {
                        str = str.Substring("allow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("allow",str));
                    }
                    if (str.StartsWith("allowed"))
                    {
                        str = str.Substring("allowed:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("allow", str));
                    }
                    if (str.StartsWith("disallow"))
                    {
                        str = str.Substring("disallow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("disallow", str));
                    }
                    if (str.StartsWith("disallow"))
                    {
                        str = str.Substring("disallow:".Length);
                        str = str.Trim();
                        robList.Add(new robotRestriction("disallow", str));
                    }
                    if (str.StartsWith("crawl-delay"))
                    {
                        str = str.Substring("crawl-delay:".Length);
                        str = str.Trim();
                        int i = 0;
                        while (i < webDelays.Count && webDelays[i].hashValue != domainHash)
                        {
                            i++;
                        }
                        if(i < webDelays.Count)
                        {
                            webDelays[i].delayValue = int.Parse(str) ;
                        }
                        else
                        {
                            webDelays.Add(new webPageDelays(domainHash, int.Parse(str)));
                        }
                        robList.Add(new robotRestriction("delay", str));
                    }
                }
            }

            return robList;
        }

        private int convertUriToHash(Uri webPage, string append)
        {
            string domainName = webPage.AbsoluteUri.Replace(webPage.AbsolutePath, "");
            if (append != "")
            {

            }
            int hashCode = domainName.GetHashCode();
            return hashCode;
        }

        private bool sendDelay(Uri webPage, List<webPageDelays> webDelays)
        {
            if(webDelays.Count > 0) {
                foreach(webPageDelays test in webDelays) {

                }

            }
            return true;
        }
    }
}
