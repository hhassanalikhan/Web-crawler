using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawlerDS
{
    class SearchInUrls
    {

        List<string> URLs;
        List<string> URLsData;
        public List<string> dic;
        System.IO.StreamReader file2 = new System.IO.StreamReader("d:\\linkContent.txt");
        System.IO.StreamReader file = new System.IO.StreamReader("d:\\LumsLinks.txt");
        System.IO.StreamReader dictionary = new System.IO.StreamReader("d:\\wordsDic.txt");
        
        string line;
        public void setupData()
        {
            URLs = new List<string>();
            URLsData = new List<string>();
            dic = new List<string>();


            while ((line = dictionary.ReadLine()) != null)
                dic.Add(line);
            dictionary.Close();
            while ((line = file.ReadLine()) != null)
                URLs.Add(line);
            file.Close();
            while ((line = file2.ReadLine()) != null)
            {
                line = line.Replace("if(document.location.hash=='#admission_criteria'){ 			$(\"#admission_criteria\").addClass(\"active\"); 			$(\"#overview\").removeClass(\"active\"); 			 			$(\"#li_admission_criteria\").addClass(\"active\"); \"}", "");

                int index = line.IndexOf("End Navbar");
                if(index > 0)
                    line.Remove(0, index);
                URLsData.Add(line);
            }
            file2.Close();


        }

        public List<string> getSearchResults(string p)
        {
            List<string> records = new List<string>();
            string[] words = p.Split(' ');
        
            for (int i = 0; i < URLsData.Count; i++)
            {
                int x=1;
                for (int j = 0; j < words.Length; j++)
                { 
                    if(x == 1)
                    {
                        if (!URLsData[i].Contains(words[j]))
                            x = 0;
                    }
                }

                if (x == 1)
                {
                    records.Add(URLs[i]);
                }

            }
                return records;

        }
    }
}
