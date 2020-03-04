using SQLite;
using System;

namespace HeartBelief.Models
{
    public abstract class DbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateLastModified { get; set; }
    }
}
