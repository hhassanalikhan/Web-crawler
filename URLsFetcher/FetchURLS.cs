
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication8
{
    class FetchURLS
    {
    
        public List<string> urls = new List<string>();
        public List<string> backupList = new List<string>();
        WebRequest request;
        WebResponse response;
        Stream dataStream;
        StreamReader reader;
        string responseFromServer;

        public void Beta()
        {
            
            //Get All hrefs in lums.edu.pk
            request = WebRequest.Create("http://lums.edu.pk/");
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            response = request.GetResponse();
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            reader = new StreamReader(dataStream);
            // Read the content.
            responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            StringCollection links = new StringCollection();

            MatchCollection AnchorTags = Regex.Matches(responseFromServer, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

            foreach (Match AnchorTag in AnchorTags)
            {
                string value = AnchorTag.Groups[1].Value;

                Match HrefAttribute = Regex.Match(value, @"href=\""(.*?)\""",
                    RegexOptions.Singleline);
                if (HrefAttribute.Success)
                {
                    string HrefValue = HrefAttribute.Groups[1].Value;
                    if (!links.Contains(HrefValue))
                    {
                        links.Add(HrefValue);
                    }
                }
            }
            //add hrefs to URLS list 
            for (int i = 0; i < links.Count; i++)
                urls.Add(links[i].ToString());
            

            //remove external links
            for (int i = 0; i < urls.Count; i++)
              if (!urls[i].ToString().Contains("lums."))
                    urls[i] = "-";
         

            //remove redundant links
            urls = urls.Distinct().ToList();
            Console.WriteLine("Size new new" + urls.Count);
          

            fetchInternalURLS();


       
            int x = 10;
        }

         


        public void fetchInternalURLS()
        {
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("d:\\linkContent.txt");
            System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\LumsLinks.txt");

            for (int i1 = 0; i1 < urls.Count; i1++)
            {
                //avoid https links,avoid unauthorized access
                if (!urls[i1].Contains("https") && !urls[i1].Contains("mailto") && !urls[i1].Equals("http://lums.edu.pk/graduate-programmes/phd-biology?admissioncriteria"))
                {
                //Get All hrefs in lums.edu.pk
                string s1 = urls[i1];
                if (!urls[i1].ToString().Contains("%20"))
                    urls[i1].Replace("%20", "");
                    try
                {



                Console.WriteLine(i1);
                request = WebRequest.Create(urls[i1]);
                request.Timeout = 300;
 
                // Get the response.
                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                Console.WriteLine("reader done");
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                Console.WriteLine("string done");
                reader.Close();
                response.Close();


                responseFromServer = Regex.Replace(responseFromServer, @"\r\n?|\n", " ");

                //find all hrefs in specific page
                if (!responseFromServer.Equals("") && responseFromServer.Contains("href"))
                {
                    /* var hrefList = doc.DocumentNode.SelectNodes("//a")
                                       .Select(p => p.GetAttributeValue("href", "not found"))
                                       .ToList();
                     doc = null;*/
                    file.WriteLine(urls[i1]);
                    file2.WriteLine(responseFromServer);
                    StringCollection links = new StringCollection();

                    MatchCollection AnchorTags = Regex.Matches(responseFromServer, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
                    int elapsed = 0;
                    //Console.WriteLine(AnchorTags.Count);
                    foreach (Match AnchorTag in AnchorTags )
                    {
                        if (elapsed < 2000)
                        {
                            string value = AnchorTag.Groups[1].Value;
                            
                            Match HrefAttribute = Regex.Match(value, @"href=\""(.*?)\""",
                                RegexOptions.Singleline);
                            if (HrefAttribute.Success)
                            {
                                string HrefValue = HrefAttribute.Groups[1].Value;
                                if (!links.Contains(HrefValue))
                                {
                                    links.Add(HrefValue);
                                }
                            }
                            //Thread.Sleep(1);
                            elapsed += 15;
                        }
                        else
                            break;
                    }

                    //add hrefs to URLS list 
                    for (int i = 0; i < links.Count; i++)
                        urls.Add(links[i].ToString());


                    //remove external links
                    for (int i = 0; i < urls.Count; i++)
                        if (!urls[i].ToString().Contains("lums.") || urls[i].ToString().Contains("lums.edu.pk/graduate-programmes/") || urls[i].ToString().Contains("portal.lums.edu") || !urls[i].ToString().Contains("http"))
                            urls[i] = "-";


                   

                    //remove redundant links
                    urls = urls.Distinct().ToList();
                    urls.Remove("-");
                    urls.Remove("http://lums.edu.pk/showcase");
                    urls.Remove("http:///vpn.lums.edu.pk/%2BCSCOE%2B/logon.html");
                
                    urls.Remove("apply-lums.php");

                   
                    int g = 0;
                    if (i1 > 800)
                    {
                        Console.WriteLine(i1);
                      //  for (int i = 0; i < i1; i++)
                          //  backupList.Add(urls[i]);
                        //urls.RemoveRange(0, i1);
                      //  i1 = 0;
                    }
                    Console.WriteLine("Size new new" + urls.Count);
                    if (urls.Count == 3603)
                        i1 = i1 + 20;
                }
                       
                }
                    catch (WebException ex)
                    {
                        file.WriteLine(urls[i1]);
                        file2.WriteLine(urls[i1]);
                        if (ex.Status == WebExceptionStatus.ProtocolError &&
                            ex.Response != null)
                        {
                            var resp = (HttpWebResponse)ex.Response;
                            if (resp.StatusCode == HttpStatusCode.NotFound)
                            {
                                
                                reader.Close();
                                response.Close();

                                i1 = i1 + 20;
                            }
                            else
                            {
                                // Do something else
                            }
                        }
                        else
                        {
                            // Do something else
                        }
                    }
                    catch (TimeoutException e)
                    {
                        file.WriteLine(urls[i1]);
                        file2.WriteLine(urls[i1]);
                        i1 = i1 + 20;
                        reader.Close();
                        response.Close();
                    }
                }
                else
                    i1 = i1 + 20; 
            }
            
            file.Close();
            file2.Close();
        }
    }
}
