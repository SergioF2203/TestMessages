using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TestMessages.Models.Mess;


namespace TestMessages.Models.Actions
{
    public class Methods
    {
        /// <summary>
        /// To Converts DateTime format to Unix format
        /// </summary>
        /// <param name="dateTime">DateTime Date</param>
        /// <returns>
        /// int converted Date to Unix format
        /// </returns>
        public static int UnixTimeStamp(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = dateTime - origin;
            return (int)Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// Generate new Id by GUID structure
        /// </summary>
        /// <returns>
        /// string Id for Customer
        /// </returns>
        public static string NewId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Create a dictionary with Id and count of messages
        /// </summary>
        /// <param name="messagesList">current messages list</param>
        /// <returns>Dictionary with customer Id and cout of messages</returns>
        public static Dictionary<string, int> MessagesCount(List<Message> messagesList)
        {
            Dictionary<string, int> mesCount = new Dictionary<string, int>();

            for (int i = 0; i < messagesList.Count; i++)
            {
                int count = 1;
                for (int j = i + 1; j < messagesList.Count; j++)
                {

                    if (messagesList[i].UserId == messagesList[j].UserId)
                        count++;
                }

                if (!mesCount.ContainsKey(messagesList[i].UserId))
                    mesCount.Add(messagesList[i].UserId, count);
            }

            return mesCount;
        }

        /// <summary>
        /// Check dictionary by id with current value
        /// </summary>
        /// <param name="mesCount">Current Dictionary</param>
        /// <param name="_value">Current value</param>
        /// <returns>
        /// string Id
        /// </returns>
        public static string ExistId(Dictionary<string, int> mesCount, int _value)
        {
            string id = null;
            foreach (KeyValuePair<string, int> keyValue in mesCount)
            {
                if (keyValue.Value == _value)
                    id = keyValue.Key;
            }

            return id;
        }

        /// <summary>
        /// Modifites a list depends conditions: max limit messages, max limit customer messages.
        /// </summary>
        /// <param name="messagesList">Current list of messages</param>
        /// <returns>
        /// modifited or not current list of messages
        /// </returns>
        public static List<Message> ModifitedList(List<Message> messagesList)
        {
            int maxLimitCustomerMessages;
            int maxLimitTotalMessages;

            try
            {
                maxLimitCustomerMessages = int.Parse(ConfigurationManager.AppSettings["cutomerMessagesLimit"]);
            }
            catch
            {
                maxLimitCustomerMessages = 10;
            }

            try
            {
                maxLimitTotalMessages = int.Parse(ConfigurationManager.AppSettings["totalLimitMessages"]);
            }
            catch
            {
                maxLimitTotalMessages = 20;
            }

            if (messagesList.Count > maxLimitTotalMessages - 1)
            {
                var mesDic = MessagesCount(messagesList);
                if (mesDic.ContainsValue(maxLimitCustomerMessages))
                {
                    var id = Methods.ExistId(mesDic, 2);

                    var result = messagesList.Find(
                        delegate (Message mes)
                        {
                            return mes.UserId == id;
                        });

                    messagesList.Remove(result);
                }
                else
                {
                    messagesList.RemoveAt(0);
                }
            }

            return messagesList;
        }

        /// <summary>
        /// Sorted list by Date
        /// </summary>
        /// <param name="list">curent list</param>
        /// <returns>
        /// MOdifited soreted list by Date
        /// </returns>
        public static List<Message> SortDate(List<Message> list)
        {
            Message temp;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j + 1].TimeStamp < list[j].TimeStamp)
                    {
                        temp = list[j + 1];
                        list[j + 1] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }
    }
}