using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace MyWPService
{
    /// <summary>
    /// Summary description for WPService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WPService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            JArray array = new JArray();
            array.Add("Manual text");
            array.Add(new DateTime(2000, 5, 23));

            JObject json = new JObject();
            json["MyArray"] = array;

            return json.ToString();
        }

        [WebMethod]
        public string HelloNumber(Int32 broj)
        {
            JArray array = new JArray();
            array.Add("The number you sent");
            array.Add(broj);

            JObject json = new JObject();
            json["MyArray"] = array;

            return json.ToString();
        }

        [WebMethod]
        public string getMainCategories()
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT * FROM MainCategories";
            int error = 0;
            string errMessage="";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["MainCategories"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }

        [WebMethod]
        public string getCategories(int mainCategory)
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT * FROM Categories WHERE MainCategoryId="+mainCategory.ToString();
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["MainCategoryId"] = reader["MainCategoryId"].ToString();
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Categories"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }


        [WebMethod]
        public string getApplicationsByCategory(int categoryId, string userId="-1")
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;


            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM (SELECT * FROM Applications WHERE Id IN (SELECT ApplicationId FROM ApplicationCategoryMap WHERE CategoryId=" + categoryId.ToString()+")) AS apps, Publishers as p WHERE p.Id=apps.PublisherId";
            
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();
                    if (userId.Equals("-1"))
                        obj["ShowLike"] = "0";
                    else
                        obj["ShowLike"] = getLikes(Int32.Parse(reader["Id"].ToString()), Int32.Parse(userId));
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Applications"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }


        [WebMethod]
        public string getApplicationById(int Id)
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM Applications AS apps, Publishers as p WHERE p.Id=apps.PublisherId and apps.Id="+Id.ToString();
            int error = 0;
            string errMessage = "";
            JObject obj = new JObject();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();

                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Application"] = obj;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }



        [WebMethod]
        public string getPublishers()
        {
            JArray array = new JArray();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT * FROM Publishers";
            int error = 0;
            string errMessage = "";
            
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Publishers"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }

        [WebMethod]
        public string getApplicationsByPublisher(int publisherId, string userId="-1")
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM (SELECT * FROM Applications WHERE Id IN (SELECT Id FROM Applications WHERE PublisherId=" + publisherId.ToString() + ")) AS apps, Publishers as p WHERE p.Id=apps.PublisherId";
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();
                    if (userId.Equals("-1"))
                        obj["ShowLike"] = "0";
                    else
                        obj["ShowLike"] = getLikes(Int32.Parse(reader["Id"].ToString()), Int32.Parse(userId));
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Applications"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }


        [WebMethod]
        public string getApplicationsByReviews(int from, int to, string userId = "-1")
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM (SELECT * FROM Applications WHERE Id IN (SELECT Id FROM Applications WHERE Reviews>=" + from.ToString() + " and Reviews<"+to.ToString()+")) AS apps, Publishers as p WHERE p.Id=apps.PublisherId";
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();
                    if (userId.Equals("-1"))
                        obj["ShowLike"] = "0";
                    else
                        obj["ShowLike"] = getLikes(Int32.Parse(reader["Id"].ToString()), Int32.Parse(userId));
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Applications"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }

        [WebMethod]
        public string getApplicationsByName(string searchText, string userId="-1")
        {
            JArray array = new JArray();


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM (SELECT * FROM Applications WHERE Id IN (SELECT Id FROM Applications WHERE Name LIKE '%"+searchText+"%')) AS apps, Publishers as p WHERE p.Id=apps.PublisherId";
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();
                    if (userId.Equals("-1"))
                        obj["ShowLike"] = "0";
                    else
                        obj["ShowLike"] = getLikes(Int32.Parse(reader["Id"].ToString()), Int32.Parse(userId));
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Applications"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }

        [WebMethod]
        public string getApplicationsByRecommendation(string userId = "-1")
        {
            JArray array = new JArray();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT apps.*, p.Name as PublisherName FROM (SELECT TOP 10 * FROM Applications WHERE Id not in (Select ApplicationId from Likes where UserId=" + userId + ") ORDER BY Reviews DESC) as apps, Publishers as p WHERE p.Id=apps.PublisherId";
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Price"] = reader["Price"].ToString();
                    obj["Rating"] = reader["Rating"].ToString();
                    obj["Reviews"] = reader["Reviews"].ToString();
                    obj["DatePublished"] = reader["DatePublished"].ToString();
                    obj["PublisherName"] = reader["PublisherName"].ToString();
                    obj["ImageUrl"] = reader["ImageUrl"].ToString();
                    if (userId.Equals("-1"))
                        obj["ShowLike"] = "0";
                    else
                        obj["ShowLike"] = getLikes(Int32.Parse(reader["Id"].ToString()), Int32.Parse(userId));
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Applications"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }


        [WebMethod]
        public string getUser(string username, string password)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT * FROM Users WHERE Username='"+username+"' AND Password='"+password+"'";
            int error = 0;
            string errMessage = "";
            JObject obj = new JObject();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    obj["Id"] = reader["Id"].ToString();
                    obj["Name"] = reader["Name"].ToString();
                    obj["Surname"] = reader["Surname"].ToString();
                    obj["Username"] = reader["Username"].ToString();
                    i++;
                }
                if (i > 0)
                    obj["valid"] = "1";
                else
                    obj["valid"] = "0";
            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["User"] = obj;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }

        private int getLikes(int appId, int userId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT COUNT(*) as l FROM Likes WHERE UserId=" + userId.ToString() + " AND ApplicationId=" + appId.ToString() + "";
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return 1-Int32.Parse(reader["l"].ToString());
                    
                }
            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            return 0;
        }


        [WebMethod]
        public int setLike(string userId, string appId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "INSERT INTO Likes(ApplicationId, UserId) VALUES("+appId+", "+userId+")";
            int error = 0;
            string errMessage = "";
            JObject obj = new JObject();
            try
            {
                connection.Open();
                int i = command.ExecuteNonQuery();
                return i;
            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {
                connection.Close();
            }

            return 0;
        }


        [WebMethod]
        public string registerUser(string firstName, string lastName, string username, string password)
        {

            if (!checkUsername(username))
            {
                JObject obj1 = new JObject();
                JObject obj2 = new JObject();
                obj2["valid"] = "0";
                obj1["User"] = obj2;
                return obj1.ToString();
            }

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "INSERT INTO Users(Name, Surname, Username, Password) VALUES('" + firstName + "', '" + lastName + "', '"+username+"', '"+password+"')";
            int error = 0;
            string errMessage = "";
            JObject obj = new JObject();
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {
                connection.Close();
            }

            return getUser(username, password);
        }



        private bool checkUsername(string username){
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "SELECT * FROM Users WHERE Username='"+username+"'";
            int error = 0;
            string errMessage = "";
            JObject obj = new JObject();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    i++;
                }
                return (i == 0);
            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {
                connection.Close();
            }

            return false;
        
        }


        [WebMethod]
        public string getComments(string appId)
        {
            JArray array = new JArray();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            SqlCommand command = new SqlCommand();
            command.Connection = connection;



            command.CommandText = "Select * from Reviews where ApplicationId="+appId;
            int error = 0;
            string errMessage = "";
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JObject obj = new JObject();
                    obj["Id"] = reader["Id"].ToString();
                    obj["Username"] = reader["UserName"].ToString();
                    obj["DateAdded"] = reader["DateAdded"].ToString();
                    obj["Text"] = reader["Text"].ToString();
                    array.Add(obj);
                }

            }
            catch (Exception err)
            {
                error = 1;
                errMessage = err.Message;

            }
            finally
            {

                connection.Close();
            }

            JObject json = new JObject();
            json["Comments"] = array;
            if (error == 1)
                return errMessage;
            else
                return json.ToString();
        }



    }
}
