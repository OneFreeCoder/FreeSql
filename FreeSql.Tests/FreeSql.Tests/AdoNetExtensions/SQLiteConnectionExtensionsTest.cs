using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Xunit;

namespace FreeSql.Tests.AdoNetExtensions.SQLiteConnectionExtensions {
	public class Methods {

		string _connectString = "Data Source=|DataDirectory|/document.db;Attachs=xxxtb.db;Pooling=true;Max Pool Size=5";

		public Methods() {
			g.sqlite.CodeFirst.SyncStructure<TestConnectionExt>();
		}

		[Fact]
		public void Insert() {
			var affrows = 0;
			using (var conn = new SQLiteConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testinsert" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Update() {
			var affrows = 0;
			using (var conn = new SQLiteConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testupdate" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				item = conn.Select<TestConnectionExt>().First();
				affrows = conn.Update<TestConnectionExt>().SetSource(item).Set(a => a.title, "testupdated").ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Delete() {
			var affrows = 0;
			using (var conn = new SQLiteConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testdelete" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				affrows = conn.Delete<TestConnectionExt>().Where(item).ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Select() {
			var list = new List<TestConnectionExt>();
			var affrows = 0;
			using (var conn = new SQLiteConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testselect" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				list = conn.Select<TestConnectionExt>().Where(a => a.id == item.id).ToList();
				conn.Close();
			}
			Assert.Single(list);
		}

		class TestConnectionExt {
			public Guid id { get; set; }
			public string title { get; set; }
			public DateTime createTime { get; set; } = DateTime.Now;
		}
	}
}
