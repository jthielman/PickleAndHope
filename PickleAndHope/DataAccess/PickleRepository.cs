using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using PickleAndHope.Models;

namespace PickleAndHope.DataAccess
{
    public class PickleRepository
    {

        const string ConnectionString = "Server = localhost; Database = PickleAndHope; Trusted_Connection = True;";

        public Pickle Add(Pickle pickle)
        {
            //pickle.Id = _pickles.Max(x => x.Id) + 1;
            //_pickles.Add(pickle);

            var sql = @"insert into Pickle(NumberInStock, Price, Size, Type)
                        output inserted.*
                        values(@NumberInStock, @Price, @Size, @Type)";

            using(var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirstOrDefault<Pickle>(sql, pickle);
                return result;
                //db.Open();

                //var cmd = db.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Price", pickle.Price);
                //cmd.Parameters.AddWithValue("Size", pickle.Size);
                //cmd.Parameters.AddWithValue("Type", pickle.Type);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    var newPickle = MapReaderToPickle(reader);
                //    return newPickle;
                //}

                //return null;
            }


        }

        public void Remove(string type)
        {
            throw new NotImplementedException();
        }

        public Pickle Update(Pickle pickle)
        {
            //var pickleToupdate = GetByType(pickle.Type);

            //pickleToupdate.NumberInStock += pickle.NumberInStock;

            //return pickleToupdate;

            var sql = @"update Pickle
                        set NumberInStock = NumberInStock + @NewStock
                        where Id = @Id";

            using (var db = new SqlConnection(ConnectionString))
            {
                var parameters = new
                {
                    NewStock = pickle.NumberInStock,
                    Id = pickle.Id
                };

                return db.QueryFirstOrDefault<Pickle>(sql, parameters);

                //db.Open();

                //var cmd = db.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NewStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Id", pickle.Id);

                //var reader = cmd.ExecuteReader();
                //if (reader.Read())
                //{
                //    var updatedPickle = MapReaderToPickle(reader);

                //    return updatedPickle;
                //}

                //return null;
            }
        }

        public Pickle GetByType(string type)
        {
            var query = @"select *
                          from Pickle
                          where Type = @Type";
            //Sql connection
            using (var db = new SqlConnection(ConnectionString))
            {
                var pickle = db.QueryFirstOrDefault<Pickle>(query, new { Type = type });

                return pickle;

                //connection.Open();


                ////Sql Command
                //var cmd = connection.CreateCommand();
                //cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("Type", type);

                ////execute the command
                //var reader = cmd.ExecuteReader();

                //if (reader.Read()) //rough equivalent to FirstOrDefault
                //{
                //    var pickle = MapReaderToPickle(reader);

                //    return pickle;
                //}

                //return null;
            }
        }

        public IEnumerable<Pickle> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Pickle>("select * from pickle");
            }

            ////Sql connection
            //var connection = new SqlConnection(ConnectionString);
            //connection.Open();

            ////Sql Command
            //var cmd = connection.CreateCommand();
            //cmd.CommandText = "select * from pickle";

            ////sql data reader - get results
            //var reader = cmd.ExecuteReader();

            //var pickles = new List<Pickle>();

            //while (reader.Read())
            //{
            //    var pickle = MapReaderToPickle(reader);

            //    pickles.Add(pickle);
            //}

            //connection.Close();

            //return pickles;
        }

        public Pickle GetById(int id)
        {
            var query = @"
                        select *
                        from Pickle
                        where Id = @id";

            using(var db = new SqlConnection(ConnectionString))
            {
                var pickle = db.QueryFirstOrDefault<Pickle>(query, new { Id = id });
                return pickle;

                //connection.Open();

                //var cmd = connection.CreateCommand();

                //cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("id", id);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    return MapReaderToPickle(reader);
                //}

                //return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            var pickle = new Pickle
            {
                Id = (int)reader["Id"],
                Type = (string)reader["Type"],
                Price = (decimal)reader["Price"],
                NumberInStock = (int)reader["NumberInStock"],
                Size = (string)reader["Size"]
            };

            return pickle;
        }
    }
}
