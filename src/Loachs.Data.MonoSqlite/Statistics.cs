﻿using System;
using System.Collections.Generic;
using System.Text;
using Mono.Data.Sqlite;
using System.Data;
using Loachs.Entity;
using Loachs.Data;


namespace  Loachs.Data.MonoSqlite
{
    public class Statistics : IStatistics
    {
        public bool UpdateStatistics(StatisticsInfo statistics)
        {
            string cmdText = @"update Loachs_Sites set 
                                PostCount=@PostCount,
                                CommentCount=@CommentCount,
                                VisitCount=@VisitCount,
                                TagCount=@TagCount";
            SqliteParameter[] prams = {
					                        SqliteHelper.MakeInParam("@PostCount", DbType.Int32,4,statistics.PostCount),
					                        SqliteHelper.MakeInParam("@CommentCount", DbType.Int32,4,statistics.CommentCount),
					                        SqliteHelper.MakeInParam("@VisitCount", DbType.Int32,4,statistics.VisitCount),
					                        SqliteHelper.MakeInParam("@TagCount", DbType.Int32,4,statistics.TagCount),
                                        };

            return SqliteHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams) == 1;
        }

        public StatisticsInfo GetStatistics()
        {
            //string cmdText = "select top 1 * from [Loachs_Sites]";
            string cmdText = "select  * from Loachs_Sites limit 0,1";


            string insertText = "insert into Loachs_Sites (PostCount,CommentCount,VisitCount,TagCount,setting) values ( '0','0','0','0','<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></SettingInfo>')";

            List<StatisticsInfo> list = DataReaderToList(SqliteHelper.ExecuteReader(cmdText));

            if (list.Count == 0)
            {
                SqliteHelper.ExecuteNonQuery(insertText);
            }
            list = DataReaderToList(SqliteHelper.ExecuteReader(cmdText));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>TermInfo</returns>
        private static List<StatisticsInfo> DataReaderToList(SqliteDataReader read)
        {
            List<StatisticsInfo> list = new List<StatisticsInfo>();
            while (read.Read())
            {
                StatisticsInfo _site = new StatisticsInfo();

                _site.PostCount = Convert.ToInt32(read["PostCount"]);
                _site.CommentCount = Convert.ToInt32(read["CommentCount"]);
                _site.VisitCount = Convert.ToInt32(read["VisitCount"]);
                _site.TagCount = Convert.ToInt32(read["TagCount"]);

                list.Add(_site);
            }
            read.Close();
            return list;
        }
    }
}
