using System;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace Postman_runner
{
    class Locate_collection
    {
        string collection_title;
        string collection_total_tests;
        string collection_total_time;
        string request_name;
        string request_total_test;
        string request_total_failures;
        string request_total_errors;
        string testcase_name;
        string testcase_time;
        public string FindCollection(string runnerFile)
        {
            string location = Path.GetFullPath(runnerFile);
            Console.WriteLine(location);
            return location;
        }
        public string Collection()
        {
            DirectoryInfo d = new DirectoryInfo(@"src\");
            FileInfo[] Files = d.GetFiles("*.json");

            FileInfo coll = Files[0];
            string collection = coll.FullName;

            return collection;

        }
        public string Environment()
        {
            DirectoryInfo d = new DirectoryInfo(@"src\");
            FileInfo[] Files = d.GetFiles("*.json");

            FileInfo env = Files[1];
            string environment = env.FullName;

            return environment;

        }

        public void EnterCmd(string collection_name)
        {
            var conn = DCU.Data.SQL.ANY.getConnection("QA_AUTOMATION");
            conn.Open();
            SqlCommand comman = new SqlCommand($"SELECT [Collection_Value],[Environment_Value] FROM [QA_Automation].[dbo].[Collection_Details] where Collection_Name='{collection_name}'", conn);
            SqlDataReader reader = comman.ExecuteReader();
            reader.Read();
            //   Console.WriteLine("Collection_value- " + Convert.ToString(reader[0]));
            //  Console.WriteLine("\n\n\nEnvironment_value- " + Convert.ToString(reader[1]));

           
            string collection = Convert.ToString(reader[0]);
            string environment = Convert.ToString(reader[1]);
            conn.Close();
            File.WriteAllText(@".\src\collection.json", collection);
            File.WriteAllText(@".\src\environment.json", environment);
            string x = "collection.json";
            string y = "environment.json";

            //string command = $"/C cd src&newman run {x} -e {y} --reporters cli,html --reporter-html-export ../Api_Report.html";

            string command = $"/C cd src&newman run {x} --insecure -e {y} --reporters cli,junit --reporter-junit-export ../ApiReport.xml";

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", command);

            //processStartInfo.RedirectStandardOutput = true;
            //processStartInfo.UseShellExecute = false;
            //processStartInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();
            // string result = process.StandardOutput.ReadToEnd();
            // Console.WriteLine(result);
           // conn.Close();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("ApiReport.xml");



            // TestResult myResultList = new TestResult();

                foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
                {
                try { 


                    //Console.WriteLine(xDoc.DocumentElement.Attributes["name"].Value);
                    //Console.WriteLine(xDoc.DocumentElement.Attributes["time"].Value);
                    //Console.WriteLine(xDoc.DocumentElement.Attributes["tests"].Value);

                    //Console.WriteLine(node.Attributes["name"].Value);
                    //Console.WriteLine(node.Attributes["tests"].Value);
                    //Console.WriteLine(node.Attributes["failures"].Value);
                    //Console.WriteLine(node.Attributes["errors"].Value);

                    collection_title = xDoc.DocumentElement.Attributes["name"].Value;

                    collection_total_tests = xDoc.DocumentElement.Attributes["tests"].Value;
                    collection_total_time = xDoc.DocumentElement.Attributes["time"].Value;

                    request_name = node.Attributes["name"].Value;
                    request_total_test = node.Attributes["tests"].Value;
                    request_total_failures = node.Attributes["failures"].Value;
                    request_total_errors = node.Attributes["errors"].Value;




                    //TestResult result = new TestResult();
                    //result.collection_title = collection_title;
                    //myResultList.Add(result);


                    foreach (XmlNode locNode in node)
                    {
                        Console.WriteLine(collection_title);
                        Console.WriteLine(collection_total_tests);
                        Console.WriteLine(collection_total_time);
                        Console.WriteLine(request_name);
                        Console.WriteLine(request_total_test);
                        Console.WriteLine(request_total_failures);
                        Console.WriteLine(request_total_errors);

                        //Console.WriteLine(locNode.Attributes["name"].Value);
                        //Console.WriteLine(locNode.Attributes["time"].Value);

                        testcase_name = locNode.Attributes["name"].Value;
                        testcase_time = locNode.Attributes["time"].Value;


                        Console.WriteLine(testcase_name);
                        Console.WriteLine(testcase_time);


                        var conn1 = DCU.Data.SQL.ANY.getConnection("QA_Automation");
                        conn1.Open();
                        SqlCommand insertCommand = new SqlCommand("INSERT INTO [QA_Automation].[dbo].[Api_Collection_TestResults] (Collection_Title, Collection_Total_Test, TimeElaspsed_Collection_Run, [Request_Name], [Request_Total_Test], [Request_Total_Failures],[Request_Total_Errors],[TestCase_Name],[TestCase_Time]) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)", conn1);

                        insertCommand.Parameters.Add(new SqlParameter("0", collection_title));
                        insertCommand.Parameters.Add(new SqlParameter("1", collection_total_tests));
                        insertCommand.Parameters.Add(new SqlParameter("2", collection_total_time));
                        insertCommand.Parameters.Add(new SqlParameter("3", request_name));
                        insertCommand.Parameters.Add(new SqlParameter("4", request_total_test));
                        insertCommand.Parameters.Add(new SqlParameter("5", request_total_failures));
                        insertCommand.Parameters.Add(new SqlParameter("6", request_total_errors));
                        insertCommand.Parameters.Add(new SqlParameter("7", testcase_name));
                        insertCommand.Parameters.Add(new SqlParameter("8", testcase_time));
                        insertCommand.ExecuteNonQuery();
                        conn1.Close();

                    }
                }
                catch (NullReferenceException ex)
                {
                    continue;
                }
            }

                }
            }
        }

    
