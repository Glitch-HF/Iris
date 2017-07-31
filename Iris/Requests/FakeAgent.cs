using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Iris.Requests
{
    public class FakeAgent
    {
        /// <summary>
        /// The current list of user agents.
        /// </summary>
        private List<UserAgent> fullAgentList;
        
        public List<UserAgent> New
        {
            get
            {
                if(fullAgentList == null)
                    fullAgentList = JsonConvert.DeserializeObject<List<UserAgent>>(Properties.Resources.agent_list);

                UserAgent[] temp = new UserAgent[fullAgentList.Count];
                fullAgentList.CopyTo(temp);

                return temp.ToList();
            }
        }
    }

    public class UserAgent
    {
        public string AgentString;

        public string AgentType;

        public string AgentName;

        public string OSType;

        public string OSName;

        public string DeviceType;
    }

    static class UserAgentListExtensions
    {
        /// <summary>
        /// For random selections.
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Filters the agents by Agent Type.
        /// </summary>
        /// <param name="agentList">The current list.</param>
        /// <param name="search">The type of agent to filter by (Crawler, Browser, Console).</param>
        /// <returns>The filtered list.</returns>
        public static List<UserAgent> AgentType(this List<UserAgent> agentList, string search)
        {
            return agentList.Where(agent => agent.AgentType.ToLower() == search.ToLower()).ToList();
        }

        /// <summary>
        /// Filters the agents by OS Type.
        /// </summary>
        /// <param name="agentList">The current list.</param>
        /// <param name="search">The type of OS to filter by (Windows, Android, Playstation).</param>
        /// <returns>The filtered list.</returns>
        public static List<UserAgent> OSType(this List<UserAgent> agentList, string search)
        {
            return agentList.Where(agent => agent.OSType.ToLower() == search.ToLower()).ToList();
        }

        /// <summary>
        /// Filters the agents by Device Type.
        /// </summary>
        /// <param name="agentList">The current list.</param>
        /// <param name="search">The type of device to filter by (Desktop, Mobile, Tablet, etc).</param>
        /// <returns>The filtered list.</returns>
        public static List<UserAgent> DeviceType(this List<UserAgent> agentList, string search)
        {
            return agentList.Where(agent => agent.DeviceType.ToLower() == search.ToLower()).ToList();
        }

        /// <summary>
        /// Returns a random UserAgent from the list.
        /// </summary>
        /// <param name="agentList">The current list.</param>
        /// <returns>A random UserAgent.</returns>
        public static UserAgent Random(this List<UserAgent> agentList)
        {
            UserAgent ret = null;
            if (agentList != null && agentList.Count() > 0)
            {
                return agentList[random.Next(agentList.Count())];
            }
            return ret;
        }
    }
}
