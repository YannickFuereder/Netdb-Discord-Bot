﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Netdb
{
    class PrefixManager
    {
        /// <summary>
        /// Setup
        /// </summary>
        public static void Setup()
        {
            var cmd = Program._con.CreateCommand();
            string command = "CREATE TABLE IF NOT EXISTS `sys`.`prefixes` (`id` INT NOT NULL AUTO_INCREMENT,`guildId` VARCHAR(10) NULL,`prefix` VARCHAR(10) NULL DEFAULT '#',PRIMARY KEY(`id`),UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,UNIQUE INDEX `guildId_UNIQUE` (`guildId` ASC) VISIBLE);";
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// Get prefix for a guild. iF never used, inserts default
        /// </summary>
        /// <param name="id">guildId</param>
        /// <returns>Prefix</returns>
        public static string GetPrefixFromGuildId(ulong id)
        {
            var cmd = Program._con.CreateCommand();
            cmd.CommandText = $"select prefix from prefixes where guildId = '{id.ToString()}'";
            var r = cmd.ExecuteReader();

            if (r.Read())
            {
                string res = r[0].ToString();
                r.Close();
                r.Dispose();
                cmd.Dispose();
                return res;
            }
            else
            {
                r.Close();
                r.Dispose();
                cmd.Dispose();
                InsertGuildPrefix(id);
                return GetPrefixFromGuildId(id);
            }
        }

        /// <summary>
        /// Private
        /// </summary>
        /// <param name="id"></param>
        private static void InsertGuildPrefix(ulong id)
        {
            var cmd = Program._con.CreateCommand();
            cmd.CommandText = $"insert into prefixes (guildId) values ('{id}');";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// ChangePrefix form Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prefix"></param>
        public static void ChangePrefixForGuild(ulong id, string prefix)
        {
            var cmd = Program._con.CreateCommand();
            cmd.CommandText = $"update prefixes set prefix = '{prefix}' where guildId = '{id}';";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}