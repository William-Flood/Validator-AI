using System;
//using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using BrainStructures;

namespace DataAccess
{
    /*public static class DBNetAccess
    {
        private static SqlConnection GetConnection()
        {
            var connString = @"Data Source=localhost;Initial Catalog=NetDB;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        }

        public static LinkedList<String> ListCollectives()
        {
            var collectiveNames = new LinkedList<String>();

            var conn = GetConnection();
            var cmdText = @"NSP_COLLECTIVE_LIST";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        collectiveNames.AddAtStart(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return collectiveNames;
        }

        public static LinkedList<NetStructure> LoadCollective(String name)
        {
            var collective = new LinkedList<NetStructure>();

            var conn = GetConnection();
            var cmdText1 = @"NSP_NETWORK_LIST";
            var cmd1 = new SqlCommand(cmdText1, conn);
            cmd1.Parameters.AddWithValue("@COLLECTIVE_NAME", name);
            cmd1.CommandType = CommandType.StoredProcedure;
            var networkTree = new BinarySearchTree<int, CompoundNode>((i,j)=>i-j);
            try
            {
                conn.Open();
                var reader = cmd1.ExecuteReader();
                while(reader.Read())
                {
                    networkTree.add(reader.GetInt32(0), new CompoundNode() {
                        Index = reader.GetInt32(0),
                        Level =reader.GetInt32(1),
                        SensoryCount = reader.GetInt32(2),
                        MotorCount = reader.GetInt32(3)
                    });
                }
            } catch
            {
                conn.Close();
                throw;
            }
            networkTree = new BinarySearchTree<int, CompoundNode>(networkTree);

            var cmdText2 = @"NSP_NODE_LIST";
            var cmd2 = new SqlCommand(cmdText2, conn);
            cmd2.Parameters.AddWithValue("@COLLECTIVE_NAME", name);
            cmd2.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                var reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    var networkAddingTo = networkTree.search(reader.GetInt32(1));
                    if (reader.IsDBNull(3))
                    {
                        networkAddingTo.NodeList[reader.GetInt32(0)] = new Neuron(20);
                    }
                    else{
                        networkAddingTo.NodeList[reader.GetInt32(0)] = new CompoundNode() { MadeFrom = reader.GetInt32(3) };
                    }

                    networkAddingTo.NodeList.MoveUp();
                }
            }
            catch
            {
                conn.Close();
                throw;
            }

            var cmdText3 = @"NSP_NODE_ROLE_LIST";
            var cmd3 = new SqlCommand(cmdText3, conn);
            cmd3.Parameters.AddWithValue("@COLLECTIVE_NAME", name);
            cmd3.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                var reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    var networkAddingTo = networkTree.search(reader.GetInt32(0));
                    var node = networkAddingTo.NodeList
                }
            }
            catch
            {
                conn.Close();
                throw;
            }



            return collective;
        }
    }*/
}
