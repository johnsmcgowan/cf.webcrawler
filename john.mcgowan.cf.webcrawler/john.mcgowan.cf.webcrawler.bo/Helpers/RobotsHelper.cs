using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Helpers
{
    public class RobotsHelper
    {
        public static bo.Web.RobotsTxt ParseRobotsTxt(string robotsTxt)
        {
            bo.Web.RobotsTxt robots = new Web.RobotsTxt();

            string[] lines = robotsTxt.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // go through the lines and parse them
            for (int i=0; i < lines.Length; i++)
            {
                if (!lines[i].Trim().StartsWith("#")) // checking for commented lines
                {

                }

            }

            string[] user_agents = Regex.Split(robotsTxt, "User-agent:");
            string userAgents = "";
            foreach (String agent in user_agents)
            {
                if (!agent.Contains("#")) // only ones where there is no comments at the beginning
                {
                    if (agent.Trim().StartsWith("*"))
                    { 
                        userAgents = agent.Trim().Substring(1);
                    }
                }
            }

            String[] disallow = Regex.Split(userAgents, "Disallow:");
            if(disallow.Length > 0)
            { 
                foreach (String item in disallow)
                {
                    if (!string.IsNullOrEmpty(item.Trim()) && item.Trim() != "\n")
                    {
                        robots.DisallowedList.Add(item.Trim());
                    }
                }
            }
            return robots;
        }
    }


}
