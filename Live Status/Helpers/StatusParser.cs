using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Live_Status.Models;
using HtmlAgilityPack;
using Windows.Globalization;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Live_Status.Helpers
{
    public static class StatusParser
    {
        public static async Task<List<status>> Parse(string content)
        {
            try
            {
                var tempList2 = new List<status>();
                string htmlPage = "";

                string locale = Language.CurrentInputMethodLanguageTag.ToString();
                string url = "https://support.xbox.com/" + locale + "/xbox-live-status?mobileOverride=mobile";

                Uri uri = new Uri((String.Format(url)));
                using (var client = new HttpClient())
                {
                    htmlPage = await client.GetStringAsync(uri);
                }

                //var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("test.txt");
                //using (var inputStream = await file.OpenReadAsync())
                //using (var classicStream = inputStream.AsStreamForRead())
                //using (var streamReader = new StreamReader(classicStream))
                //{
                //    while (streamReader.Peek() >= 0)
                //    {
                //        htmlPage += streamReader.ReadLine();
                //    }
                //}

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlPage);

                //First Part
                Func<ObservableCollection<status>> funcCalc = () =>
                {
                    ObservableCollection<status> tempList = new ObservableCollection<status>();

                    foreach (var div in htmlDocument.DocumentNode.Descendants().Where(i => i.Name == "li" && (i.GetAttributeValue("class", "").StartsWith("service"))))
                    {
                        try
                        {
                            status newStatus = new status();
                            string name;
                            string state;
                            string description;

                            name = div.Descendants().Where(i => (i.Name == "h3") && i.GetAttributeValue("class", "") == "servicename m-t-n ").FirstOrDefault().InnerText.Trim();
                            newStatus.name = name.Replace("&#233;", "é");

                            state = div.Descendants().Where(i => i.Name == "span" && (i.GetAttributeValue("class", "") == "unavailable d-ib-lg d-n-s" || i.GetAttributeValue("class", "") == "active d-ib-lg d-n-s")).FirstOrDefault().InnerText.Trim();
                            newStatus.state = state;

                            description = div.Descendants().Where(i => i.Name == "p" && (i.GetAttributeValue("class", "") == "description")).FirstOrDefault().InnerText.Trim();
                            newStatus.description = description.Replace("&amp;", "&");

                            if (div.Descendants().Where(i => i.Name == "span" && (i.GetAttributeValue("class", "") == "unavailable d-ib-lg d-n-s")).FirstOrDefault() != null)
                            {
                                newStatus.stateIcon = "\uE7BA";
                                try
                                {
                                    newStatus.affectedServices = div.Descendants().Where(i => i.Name == "ul" && (i.GetAttributeValue("class", "") == "services")).FirstOrDefault().InnerText.Trim();
                                    newStatus.lastUpdate = div.Descendants().Where(i => i.Name == "p" && (i.GetAttributeValue("class", "") == "")).Last().InnerText.Trim();
                                }
                                catch
                                {

                                }

                                newStatus.affectedPlatforms = new List<string>();
                                foreach (var div2 in div.Descendants().Where(i => i.Name == "div" && (i.GetAttributeValue("class", "").StartsWith("label"))))
                                {
                                    try
                                    {
                                        newStatus.affectedPlatforms.Add(div2.InnerText.Trim());
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                                newStatus.stateIcon = "\uE73E";

                            tempList.Add(newStatus);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    return tempList;
                };

                var list = await Task.Run(funcCalc);
                foreach (var i in list)
                    tempList2.Add(i);

                //Second Part
                Func<ObservableCollection<status>> funcCalc2 = () =>
                {
                    ObservableCollection<status> tempList = new ObservableCollection<status>();

                    foreach (var div in htmlDocument.DocumentNode.Descendants().Where(i => i.Name == "div" && (i.GetAttributeValue("class", "") == ("row l-col-24-24"))))
                    {
                        try
                        {
                            status newStatus = new status();
                            string name;
                            string state;
                            string description;

                            name = div.Descendants().Where(i => (i.Name == "h2")).FirstOrDefault().InnerText.Trim();
                            newStatus.name = name.Replace("&#233;", "é");

                            state = div.Descendants().Where(i => i.Name == "span" && (i.GetAttributeValue("class", "") == "unavailable d-ib-lg d-n-s" || i.GetAttributeValue("class", "") == "active d-ib-lg d-n-s")).FirstOrDefault().InnerText.Trim();
                            newStatus.state = state;

                            description = div.Descendants().Where(i => i.Name == "p" && (i.GetAttributeValue("class", "") == "description-only")).FirstOrDefault().InnerText.Trim();
                            newStatus.description = description;

                            if (div.Descendants().Where(i => i.Name == "span" && (i.GetAttributeValue("class", "") == "unavailable d-ib-lg d-n-s")).FirstOrDefault() != null)
                            {
                                newStatus.stateIcon = "\uE7BA";
                                try
                                {
                                    newStatus.affectedServices = div.Descendants().Where(i => i.Name == "ul" && (i.GetAttributeValue("class", "") == "services")).FirstOrDefault().InnerText.Trim();
                                    newStatus.lastUpdate = div.Descendants().Where(i => i.Name == "p" && (i.GetAttributeValue("class", "") == "")).Last().InnerText.Trim();
                                }
                                catch
                                {

                                }

                                newStatus.affectedPlatforms = new List<string>();
                                foreach (var div2 in div.Descendants().Where(i => i.Name == "div" && (i.GetAttributeValue("class", "").StartsWith("label"))))
                                {
                                    try
                                    {
                                        newStatus.affectedPlatforms.Add(div2.InnerText.Trim());
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                                newStatus.stateIcon = "\uE73E";

                            tempList.Add(newStatus);
                            //xl-col-14-24 l-col-13-24 m-col-16-24 s-col-20-24 xs-col-20-24
                            //description = div.Descendants().Where(i => (i.Name == "p") && i.GetAttributeValue("class", "") == "description").FirstOrDefault().InnerText.Trim();
                            //newStatus.description = description.Replace("&amp;","&");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    return tempList;
                };

                var list2 = await Task.Run(funcCalc2);
                foreach (var i in list2)
                    tempList2.Add(i);

                return tempList2;
            }
            catch
            {
                return null;
            }
        }
    }
}
