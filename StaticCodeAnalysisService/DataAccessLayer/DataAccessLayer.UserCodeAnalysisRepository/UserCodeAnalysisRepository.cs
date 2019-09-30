using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.SqlHelper;
using Contracts.UserCodeAnalysisRepository;
using Models.Tools;
using Models.GatingResult;

using System.Data.SqlClient;

namespace DataAccessLayer.UserCodeAnalysisRepository
{
    public class UserCodeAnalysisRepository : IUserCodeAnalysisRepository
    {
        private readonly string conString;

        public UserCodeAnalysisRepository(bool test = false)
        {
            if (test)
                conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StaticCodeAnalysisDB.Test;Integrated Security=True";
            else
                conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StaticCodeAnalysisDB;Integrated Security=True";
        }

        public bool Add(string userName, string path, string branch, Tools tool, int result,int threshold)
        {
            var query = string.Format("Insert into UserCodeAnalysisData(UserName,Repository,Branch,Tool,RunTime,Result,Threshold) values('{0}','{1}','{2}','{3}','{4}','{5}',{6})", userName, path,branch, (int)tool, DateTime.Now, result, threshold);

            if (SqlCommands.ExecuteCommand(query, conString))
            {
                return true;
            }
            return false;
        }

        public bool Delete(string userName, string path, string branch, Tools tool)
        {
            var query = string.Format("DELETE FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'AND Tool='{2}' AND Branch='{3}'", userName, path, (int)tool, branch);
            return SqlCommands.ExecuteCommand(query, conString);
        }

        public List<int> ReadAllResults(string userName, string path, string branch, Tools tool)
        {
            if (!Exists(userName, path, branch, tool))
                return new List<int>();

            return ReadAllResultsFromDB(userName, path, branch, tool);
        }

        private bool Exists(string userName, string path, string branch, Tools tool)
        {
            var query = string.Format("Select * FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'AND Tool='{2}' AND Branch='{3}'", userName, path, (int)tool, branch);
            return SqlCommands.Exists(conString, query);
        }


        private bool Exists(string userName)
        {
            var query = string.Format("Select * FROM UserCodeAnalysisData WHERE UserName='{0}'", userName);
            return SqlCommands.Exists(conString, query);
        }


        public int ReadRelativeThreshold(string userName, string path, string branch, Tools tool)
        {
            if (!Exists(userName, path, branch, tool))
                return -1;


            var query = string.Format("Select TOP 1 Result FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'AND Tool='{2}' AND Branch='{3}' order by Result ASC", userName, path, (int)tool, branch);

            int threshold=-1;
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                while (reader.Read())
                {
                    threshold=Convert.ToInt32(reader["Result"]);
                }
                reader.Close();
            }
            return threshold;


        }

        private List<int> ReadAllResultsFromDB(string userName, string path, string branch, Tools tool)
        {
            var query = string.Format("Select TOP 5 Result FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'AND Tool='{2}' AND Branch='{3}' order by RunTime DESC", userName, path, (int)tool, branch);

            List<int> results = new List<int>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                while (reader.Read())
                {
                    results.Add(Convert.ToInt32(reader["Result"]));
                }
                reader.Close();
            }
            return results;
        }
        
        public List<GatingResult> ReadRepoList(string userName)
        {
            if (!Exists(userName))
                return new List<GatingResult>();

            return ReadRecentRepoListFromDB(userName);
        }



        public List<GatingResult> ReadRecentRepoListFromDB(string userName)
        {
            var distinctRepo = GetDistinctRepo(userName);

            List<GatingResult> repos = new List<GatingResult>();

            foreach (List<string> uniqeRepo in distinctRepo)
            {
                repos.Add(ParseAnalysisReport(userName, uniqeRepo[0], uniqeRepo[1], (Tools)Convert.ToInt64(uniqeRepo[2])));
            }

            return repos;
        }


        private List<List<string>> GetDistinctRepo(string userName)
        {
            var query = string.Format("Select DISTINCT Repository,Branch,Tool FROM UserCodeAnalysisData WHERE UserName='{0}'", userName);

            List<List<string>> repos = new List<List<string>>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                while (reader.Read())
                {

                    List<string> repo = new List<string>();

                    GatingResult result = new GatingResult();
                    repo.Add(reader["Repository"].ToString());
                    repo.Add(reader["Branch"].ToString());
                    repo.Add(reader["Tool"].ToString());
                    repos.Add(repo);
                }
                reader.Close();
            }
            return repos;
        }




        private GatingResult ParseAnalysisReport(string userName,string path,string branch,Tools tool)
        {

            var query = string.Format("Select TOP 1 * FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'  AND Tool='{2}' AND Branch='{3}' order by RunTime DESC", userName, path, (int)tool, branch);

            GatingResult result = new GatingResult();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                while (reader.Read())
                {
                    result.Reository = reader["Repository"].ToString();
                    result.Branch = reader["Branch"].ToString();
                    result.Tool = (Tools)Convert.ToInt64(reader["Tool"]);
                    result.DateTime= reader["RunTime"].ToString();
                    result.NoOfError= Convert.ToInt32(reader["Result"]);
                    result.Result = GetGatingResult(Convert.ToInt32(reader["Threshold"]),result.NoOfError);
                }
                reader.Close();
            }
            return result;
        }

        private bool GetGatingResult(int threshold,int noOfErrors)
        {
            if (threshold >= noOfErrors)
                return true;
            else
                return false;

        }

        public List<int> ReadLatestResult(string userName, string path, string branch, Tools tool)
        {

            if (!Exists(userName,path, branch, tool))
                return new List<int>();

            return ReadLatestResultFromDB(userName,path,tool,branch);
        }

        private List<int> ReadLatestResultFromDB(string userName,string path,Tools tool,string branch)
        {
            var result = new List<int>();
            var query = string.Format("Select TOP 1 Threshold,Result FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'  AND Tool='{2}' AND Branch='{3}' order by RunTime DESC", userName, path, (int)tool,branch);
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                if (reader.Read())
                {

                    result.Add(Convert.ToInt32(reader["Threshold"]));
                    result.Add(Convert.ToInt32(reader["Result"]));
                }
                reader.Close();
            }

            return result;
        }


        private DateTime GetRuntime(string userName, string path, Tools tool, string branch)
        {
            DateTime time=new DateTime();
            var query = string.Format("Select TOP 1 RunTime FROM UserCodeAnalysisData WHERE UserName='{0}' AND Repository='{1}'  AND Tool='{2}' AND Branch='{3}' order by RunTime DESC", userName, path, (int)tool, branch);
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommands.OpenConnection(con);

                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                if (reader.Read())
                {
                    time = Convert.ToDateTime(reader["RunTime"]);
                }
                reader.Close();
            }

            return time;
        }


        public bool UpdateThreshold(string userName, string path, Tools tool, string branch,int threshold,int result)
        {
            if (!Exists(userName, path, branch, tool))
                return false;


            var time = GetRuntime(userName,path,tool,branch);
            var query = string.Format("UPDATE UserCodeAnalysisData SET Threshold='{0}' WHERE UserName='{1}' AND Repository='{2}'  AND Tool='{3}' AND Branch='{4}' AND Result='{5}' AND RunTime='{6}'",threshold, userName, path, (int)tool, branch,result,time);
            if (SqlCommands.ExecuteCommand(query, conString))
            {
                return true;
            }

            return false;


        }



    }
}
